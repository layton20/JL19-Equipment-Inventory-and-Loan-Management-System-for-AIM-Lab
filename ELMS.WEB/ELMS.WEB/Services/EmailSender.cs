using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration __Configuration;
        private readonly AuthMessageSenderOptions __OptionsAccessor;

        public EmailSender(IConfiguration configuration, IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            __Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            __OptionsAccessor = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(__OptionsAccessor.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(__Configuration["SendGrid:FROM_EMAIL"], __OptionsAccessor.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }

        public async Task<bool> SendEmailSampleAsync()
        {
            var apiKey = __Configuration["SendGrid:API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("lej1@aston.ac.uk", "Johnny Le");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("contact.lejohnny@gmail.com", "Johnny Le");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            Response response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }
}