using ELMS.WEB.Managers.Email.Interface;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ELMS.WEB.Jobs
{
    public class EmailJob : IJob
    {
        private readonly IServiceProvider __ServiceProvider;
        private readonly string WORKER_NAME = "Email Job Worker";

        public EmailJob(IServiceProvider serviceProvider)
        {
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"{WORKER_NAME}: Task Beginning");

            await SendScheduledEmailsAsync();

            Debug.WriteLine($"{WORKER_NAME}: Task Complete");
        }

        private async Task SendScheduledEmailsAsync()
        {
            using (var scope = __ServiceProvider.CreateScope())
            {
                IEmailScheduleManager _EmailScheduleManager = scope.ServiceProvider.GetRequiredService<IEmailScheduleManager>();
                Debug.WriteLine($"{WORKER_NAME}: Sending Scheduled Emails");
                await _EmailScheduleManager.SendScheduledEmails();
            }
        }
    }
}