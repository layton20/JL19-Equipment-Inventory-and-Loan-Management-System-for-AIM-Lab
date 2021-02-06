using ELMS.WEB.Background.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background
{
    public class BackgroundWorker : IHostedService
    {
        private readonly ILogger<BackgroundWorker> __Logger;
        private readonly IEmailWorker __EmailWorker;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, IEmailWorker emailWorker)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __EmailWorker = emailWorker ?? throw new ArgumentNullException(nameof(emailWorker));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await __EmailWorker.DoWork(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            __Logger.LogInformation("EmailJob Worker Stopping");
            return Task.CompletedTask;
        }
    }
}
