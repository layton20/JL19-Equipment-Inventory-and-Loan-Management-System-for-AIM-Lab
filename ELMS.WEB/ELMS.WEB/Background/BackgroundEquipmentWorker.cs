using ELMS.WEB.Background.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background
{
    public class BackgroundEquipmentWorker : BackgroundService
    {
        private readonly IEquipmentWorker __EquipmentWorker;

        public BackgroundEquipmentWorker(IEquipmentWorker equipmentWorker)
        {
            __EquipmentWorker = equipmentWorker ?? throw new ArgumentNullException(nameof(equipmentWorker));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await __EquipmentWorker.DoWork(stoppingToken);
            await Task.CompletedTask;
        }
    }
}
