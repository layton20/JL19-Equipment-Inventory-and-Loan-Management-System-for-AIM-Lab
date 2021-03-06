using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Concrete
{
    public class EquipmentWorker : IEquipmentWorker
    {
        private readonly ILogger __Logger;
        private readonly IServiceProvider __ServiceProvider;
        private readonly string WORKER_NAME = "EquipmentWorker";

        public EquipmentWorker(ILogger<LoanWorker> logger, IServiceProvider serviceProvider)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(await PruneDanglingEquipmentMedia());
                await Task.Delay(await UpdateWarrantyExpiredEquipment());
            }
        }

        private async Task<int> UpdateWarrantyExpiredEquipment()
        {
            int delaySeconds = 1000 * 60 * 60 * 5;

            __Logger.LogInformation($"{WORKER_NAME}: Checking for expired equipment");

            using (var scope = __ServiceProvider.CreateScope())
            {
                IEquipmentManager _EquipmentManager = scope.ServiceProvider.GetRequiredService<IEquipmentManager>();
                
                IList<EquipmentResponse> _EquipmentToUpdateStatus = (await _EquipmentManager.GetAsync()).Equipments.Where(x => x.WarrantyExpirationDate <= DateTime.Now && x.Status != Enums.Equipment.Status.WrittenOff).ToList();

                if (_EquipmentToUpdateStatus != null && _EquipmentToUpdateStatus?.Count > 0)
                {
                    __Logger.LogInformation($"{WORKER_NAME}: Switched {_EquipmentToUpdateStatus.Count} Equipment statuses to status 'WrittenOff'.");
                }
            }

            return delaySeconds;
        }

        private async Task<int> PruneDanglingEquipmentMedia()
        {
            int delaySeconds = 1000 * 60 * 60 * 12;

            __Logger.LogInformation($"{WORKER_NAME}: Checking for dangling equipment media and blobs");

            using (var scope = __ServiceProvider.CreateScope())
            {
                IEquipmentManager _EquipmentManager = scope.ServiceProvider.GetRequiredService<IEquipmentManager>();
                IEquipmentBlobManager _EquipmentBlobManager = scope.ServiceProvider.GetRequiredService<IEquipmentBlobManager>();
                IBlobManager _BlobManager = scope.ServiceProvider.GetRequiredService<IBlobManager>();
                IBlobService _BlobService = scope.ServiceProvider.GetRequiredService<IBlobService>();

                IList<Guid> _EquipmentList = (await _EquipmentManager.GetAsync()).Equipments.Select(x => x.UID).ToList();
                IList<EquipmentBlobResponse> _EquipmentBlobs = (await _EquipmentBlobManager.GetAsync()).Where(x => _EquipmentList.Contains(x.EquipmentUID)).ToList();

                if (_EquipmentBlobs != null && _EquipmentBlobs.Count > 0)
                {
                    foreach (EquipmentBlobResponse equipmentBlob in _EquipmentBlobs)
                    {
                        await _BlobService.DeleteBlobAsync(equipmentBlob.Blob.Name);
                        await _EquipmentBlobManager.DeleteAsync(equipmentBlob.UID);
                        await _BlobManager.DeleteAsync(equipmentBlob.Blob.UID);
                    }

                }

                __Logger.LogInformation($"{WORKER_NAME}: Pruned {_EquipmentBlobs?.Count ?? 0} dangling equipment-media.");
            }

            return delaySeconds;
        }
    }
}
