using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Jobs
{
    public class EquipmentJob : IJob
    {
        private readonly IServiceProvider __ServiceProvider;
        private readonly string WORKER_NAME = "Equipment Job Worker";

        public EquipmentJob(IServiceProvider serviceProvider)
        {
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"{WORKER_NAME}: Task Beginning");

            await UpdateWarrantyExpiredEquipment();
            await PruneDanglingEquipmentMedia();

            Debug.WriteLine($"{WORKER_NAME}: Task Complete");
        }

        private async Task UpdateWarrantyExpiredEquipment()
        {
            using (var scope = __ServiceProvider.CreateScope())
            {
                IEquipmentManager _EquipmentManager = scope.ServiceProvider.GetRequiredService<IEquipmentManager>();

                IList<EquipmentResponse> _EquipmentToUpdateStatus = (await _EquipmentManager.GetAsync()).Equipments.Where(x => x.WarrantyExpirationDate <= DateTime.Now && x.Status != Enums.Equipment.Status.WrittenOff).ToList();

                if (_EquipmentToUpdateStatus != null && _EquipmentToUpdateStatus?.Count > 0)
                {
                    foreach (EquipmentResponse equipmentResponse in _EquipmentToUpdateStatus)
                    {
                        await _EquipmentManager.UpdateStatusAsync(equipmentResponse.UID, Enums.Equipment.Status.WrittenOff);
                    }

                    Debug.WriteLine($"{WORKER_NAME}: Switched {_EquipmentToUpdateStatus.Count} Equipment statuses to status 'WrittenOff'.");
                }
            }
        }

        private async Task PruneDanglingEquipmentMedia()
        {
            Debug.WriteLine($"{WORKER_NAME}: Checking for dangling equipment media and blobs");

            using (var scope = __ServiceProvider.CreateScope())
            {
                IEquipmentManager _EquipmentManager = scope.ServiceProvider.GetRequiredService<IEquipmentManager>();
                IEquipmentBlobManager _EquipmentBlobManager = scope.ServiceProvider.GetRequiredService<IEquipmentBlobManager>();
                IBlobManager _BlobManager = scope.ServiceProvider.GetRequiredService<IBlobManager>();
                IBlobService _BlobService = scope.ServiceProvider.GetRequiredService<IBlobService>();

                IList<Guid> _EquipmentList = (await _EquipmentManager.GetAsync()).Equipments.Select(x => x.UID).ToList();
                IList<EquipmentBlobResponse> _EquipmentBlobs = (await _EquipmentBlobManager.GetAsync()).Where(x => !_EquipmentList.Contains(x.EquipmentUID)).ToList();

                if (_EquipmentBlobs != null && _EquipmentBlobs.Count > 0)
                {
                    foreach (EquipmentBlobResponse equipmentBlob in _EquipmentBlobs)
                    {
                        await _BlobService.DeleteBlobAsync(equipmentBlob.Blob.Name);
                        await _EquipmentBlobManager.DeleteAsync(equipmentBlob.UID);
                        await _BlobManager.DeleteAsync(equipmentBlob.Blob.UID);
                    }
                }

                Debug.WriteLine($"{WORKER_NAME}: Pruned {_EquipmentBlobs?.Count ?? 0} dangling equipment-media.");
            }
        }
    }
}
