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

        public BackgroundLoanWorker(ILoanWorker loanWorker)
        {
            __LoanWorker = loanWorker ?? throw new ArgumentNullException(nameof(loanWorker));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await __LoanWorker.DoWork(stoppingToken);
        }
    }
}