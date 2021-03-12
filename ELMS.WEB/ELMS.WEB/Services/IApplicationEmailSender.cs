using ELMS.WEB.Areas.Email.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using System.Threading.Tasks;

namespace ELMS.WEB.Services
{
    public interface IApplicationEmailSender : IEmailSender
    {
        public Task<Response> SendGeneralEmail(string email, string subject, CustomEmailTemplate templateData);
        public Task<Response> SendLoanConfirmEmail(string email, string subject, ConfirmEmailTemplate templateData);

        public Task<Response> SendLoanConfirmedEmail(string email, string subject, ConfirmedEmailTemplate templateData);

        public Task<Response> SendLoanNearlyDueEmail(string email, string subject, LoanNearlyDueTemplate templateData);

        public Task<Response> SendLoanOverdueEmail(string email, string subject, LoanOverdueTemplate templateData);

        public Task<Response> SendWarrantyExpiredEmail(string email, string subject, WarrantyExpiredTemplate templateData);

        public Task<Response> SendWarrantyNearlyExpiredEmail(string email, string subject, WarrantyNearlyExpiredTemplate templateData);
        public Task<Response> SendEmailConfirmationEmail(string email, string subject, EmailConfirmationTemplate templateData);
    }
}