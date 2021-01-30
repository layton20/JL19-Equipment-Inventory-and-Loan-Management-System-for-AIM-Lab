using ELMS.WEB.Areas.Email.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using System.Threading.Tasks;

namespace ELMS.WEB.Services
{
    public interface IApplicationEmailSender : IEmailSender
    {
        public Task<Response> Execute(string apiKey, string subject, string message, string email);
        public Task<Response> SendConfirmLoanEmail(string email, string subject, ConfirmEmailTemplate templateData);
        public Task<Response> SendConfirmedLoanEmail(string email, string subject, ConfirmedEmailTemplate templateData);
    }
}
