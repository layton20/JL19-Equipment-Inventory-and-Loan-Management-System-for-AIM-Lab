using ELMS.WEB.Background.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background
{
    public class BackgroundLoanWorker : BackgroundService
    {
        private readonly ILoanWorker __LoanWorker;
        private readonly IEquipmentWorker __EquipmentWorker;

        public BackgroundLoanWorker(ILoanWorker loanWorker, IEquipmentWorker equipmentWorker)
        {
            __LoanWorker = loanWorker ?? throw new ArgumentNullException(nameof(loanWorker));
            __EquipmentWorker = equipmentWorker ?? throw new ArgumentNullException(nameof(equipmentWorker));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await __EquipmentWorker.DoWork(stoppingToken);
            await __LoanWorker.DoWork(stoppingToken);
            await Task.CompletedTask;
        }
    }
}