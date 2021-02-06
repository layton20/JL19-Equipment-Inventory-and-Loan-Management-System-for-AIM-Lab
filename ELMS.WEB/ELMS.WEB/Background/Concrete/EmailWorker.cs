using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Concrete
{
    public class EmailWorker : IEmailWorker
    {
        private readonly ILogger<EmailWorker> __Logger;
        private readonly IApplicationEmailSender __EmailSender;
        private int number = 0;

        public EmailWorker(ILogger<EmailWorker> logger, IApplicationEmailSender emailSender)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Interlocked.Increment(ref number);
                __Logger.LogInformation("EmailWorker: Working");
                await Task.Delay(1000 * 5);
            }
        }
    }
}
