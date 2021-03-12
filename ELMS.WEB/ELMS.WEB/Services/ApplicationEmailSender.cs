using ELMS.WEB.Areas.Email.Data;
using ELMS.WEB.Enums.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Services
{
    public class ApplicationEmailSender : IApplicationEmailSender
    {
        private readonly IConfiguration __Configuration;
        private readonly SendGridEmailSenderOptions __OptionsAccessor;

        public ApplicationEmailSender(IConfiguration configuration, IOptions<SendGridEmailSenderOptions> optionsAccessor)
        {
            __Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            __OptionsAccessor = optionsAccessor.Value;
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

        public async Task<Response> SendGeneralEmail(string email, string subject, CustomEmailTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();

            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(new EmailAddress(email));
            _Message.SetSubject(subject);
            _Message.SetClickTracking(false, false);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:CUSTOM_TEMPLATE"]);
            _Message.SetTemplateData(templateData);

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

        public async Task<Response> SendLoanConfirmEmail(string email, string subject, ConfirmEmailTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:CONFIRM_LOAN"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendLoanConfirmedEmail(string email, string subject, ConfirmedEmailTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:CONFIRMED_LOAN"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendLoanNearlyDueEmail(string email, string subject, LoanNearlyDueTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:NEARLY_OVERDUE_LOAN"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendLoanOverdueEmail(string email, string subject, LoanOverdueTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:OVERDUE_LOAN"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendWarrantyExpiredEmail(string email, string subject, WarrantyExpiredTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:EXPIRED_WARRANTY"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendWarrantyNearlyExpiredEmail(string email, string subject, WarrantyNearlyExpiredTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:NEARLY_EXPIRED_WARRANTY"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public async Task<Response> SendEmailConfirmationEmail(string email, string subject, EmailConfirmationTemplate templateData)
        {
            SendGridClient _Client = new SendGridClient(__OptionsAccessor.SendGridKey);
            SendGridMessage _Message = new SendGridMessage();
            _Message.SetFrom(new EmailAddress(__OptionsAccessor.SenderEmail, __OptionsAccessor.SenderName));
            _Message.AddTo(email);
            _Message.SetTemplateId(__Configuration["SendGrid:TEMPLATES:EMAIL_CONFIRMATION"]);
            _Message.SetTemplateData(templateData);

            return await _Client.SendEmailAsync(_Message);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}