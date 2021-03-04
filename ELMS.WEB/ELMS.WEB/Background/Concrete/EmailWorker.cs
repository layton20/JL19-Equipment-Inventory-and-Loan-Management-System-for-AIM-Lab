using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Managers.Email.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Concrete
{
    public class EmailWorker : IEmailWorker
    {
        private readonly ILogger<EmailWorker> __Logger;
        private readonly IServiceProvider __ServiceProvider;
        private readonly string WORKER_NAME = "EmailWorker";

        public EmailWorker(ILogger<EmailWorker> logger, IServiceProvider serviceProvider)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(await SendScheduledEmailsAsync());
            }
        }

        private async Task<int> SendScheduledEmailsAsync()
        {
            int delaySeconds = 60000 * 60 * 2;

            __Logger.LogInformation($"{WORKER_NAME}: Sending Scheduled Emails");

            using (var scope = __ServiceProvider.CreateScope())
            {
                IEmailScheduleManager _EmailScheduleManager = scope.ServiceProvider.GetRequiredService<IEmailScheduleManager>();
                await _EmailScheduleManager.SendScheduledEmails();
            }

            __Logger.LogInformation($"{WORKER_NAME}: Sending scheduled emails complete. Task scheduled again in {delaySeconds} milliseconds.");

            return delaySeconds;
        }
    }
}