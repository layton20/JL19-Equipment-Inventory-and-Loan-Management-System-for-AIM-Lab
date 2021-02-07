using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Managers.Email.Interface;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider __Services;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, IServiceProvider services)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.__Services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            __Logger.LogInformation("EmailJob Worker Starting");

            using (var scope = __Services.CreateScope())
            {
                var _ScopedProcessingService = scope.ServiceProvider.GetRequiredService<IEmailScheduleManager>();

                await _ScopedProcessingService.SendScheduledEmails();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            __Logger.LogInformation("EmailJob Worker Stopping");
            return Task.CompletedTask;
        }
    }
}
