using ELMS.WEB.Areas.Email.Data;
using ELMS.WEB.Enums.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration __Configuration;
        private readonly SendGridEmailSenderOptions __OptionsAccessor;

        public EmailSender(IConfiguration configuration, IOptions<SendGridEmailSenderOptions> optionsAccessor)
        {
            __Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            __OptionsAccessor = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(__OptionsAccessor.SendGridKey, subject, message, email);
        }

        public async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            IList<string> _PreconfiguredTemplates = Enum.GetNames(typeof(EmailConfigurationType));

            if (_PreconfiguredTemplates.Contains(subject))
            {
                await SendConfirmLoanEmail(email, subject, message);
            }

            return await SendGeneralEmail(apiKey, subject, message, email);
        }

        public async Task<Response> SendConfirmLoanEmail(string email, string subject, string message)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:CONFIRM_LOAN"]);
            _Message.SetTemplateData(new ConfirmEmailTemplate
            {
                Confirm_Loan_URL = message
            });

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendGeneralEmail(string apiKey, string subject, string message, string email)
        {
            SendGridClient _Client = new SendGridClient(apiKey);
            SendGridMessage _Message = new SendGridMessage()
            {
                From = new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            _Message.AddTo(new EmailAddress(email));
            _Message.SetClickTracking(false, false);

            return await _Client.SendEmailAsync(_Message);
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