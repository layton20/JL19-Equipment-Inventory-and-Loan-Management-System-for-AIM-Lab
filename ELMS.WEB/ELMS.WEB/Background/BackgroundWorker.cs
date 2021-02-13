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
        private int num = 0;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, IServiceProvider services)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            __Logger.LogInformation("EmailJob Worker Starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                __Logger.LogInformation("Running scheduled email service");
                using (var scope = __Services.CreateScope())
                {
                    IEmailScheduleManager _ScopedProcessingService = scope.ServiceProvider.GetRequiredService<IEmailScheduleManager>();
                    await _ScopedProcessingService.SendScheduledEmails();
                }
                __Logger.LogInformation("Stopping scheduled email service");

                await Task.Delay(60000 * 60 * 2);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            __Logger.LogInformation("EmailJob Worker Stopping");
            return Task.CompletedTask;
        }
    }
}