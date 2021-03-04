using ELMS.WEB.Background.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background
{
    public class BackgroundEmailWorker : BackgroundService
    {
        private readonly IEmailWorker __EmailWorker;

        public BackgroundEmailWorker(IEmailWorker emailWorker)
        {
            __EmailWorker = emailWorker ?? throw new ArgumentNullException(nameof(emailWorker));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await __EmailWorker.DoWork(stoppingToken);
        }
    }
}
