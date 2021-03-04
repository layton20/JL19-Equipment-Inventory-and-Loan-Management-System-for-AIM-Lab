using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Equipment.Response;
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
    }
}
