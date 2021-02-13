using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Managers.Email.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Concrete
{
    public class EmailWorker : IEmailWorker
    {
        private readonly ILogger<EmailWorker> __Logger;
        private readonly IEmailScheduleManager __EmailScheduleManager;
        private int number = 0;

        public EmailWorker(ILogger<EmailWorker> logger, IEmailScheduleManager emailScheduleManager)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                __Logger.LogInformation("EmailWorker: Working");
                //await __EmailScheduleManager.SendScheduledEmails();
                await Task.Delay(6000 * 60 * 2);
            }
        }
    }
}