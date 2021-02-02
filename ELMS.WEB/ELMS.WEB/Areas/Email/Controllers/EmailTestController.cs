using ELMS.WEB.Areas.Email.Data;
using ELMS.WEB.Helpers;
using ELMS.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Email.Controllers
{
    [Area("Email")]
    public class EmailTestController : Controller
    {
        private readonly IApplicationEmailSender __ApplicationEmailSender;
        private const string TEST_EMAIL = "contact.lejohnny@gmail.com";

        public EmailTestController(IApplicationEmailSender applicationEmailSender)
        {
            __ApplicationEmailSender = applicationEmailSender ?? throw new ArgumentNullException(nameof(applicationEmailSender));
        }

        [HttpGet]
        public IActionResult Index(string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["Success"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["Error"] = errorMessage;
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendConfirmLoanEmail()
        {
            await __ApplicationEmailSender.SendLoanConfirmEmail(TEST_EMAIL, "AIM - Loan Confirmed", new ConfirmEmailTemplate
            {
                Confirm_Loan_URL = "https://www.google.com/"
            });

            return RedirectToAction("Index", "EmailTest", new { Area = "Email", successMessage =  $"{GlobalConstants.SUCCESS_ACTION_PREFIX} sent email." });
        }

        [HttpGet]
        public async Task<IActionResult> SendConfirmedLoanEmail()
        {
            await __ApplicationEmailSender.SendLoanConfirmedEmail(TEST_EMAIL, "AIM - Loan Confirmed", new ConfirmedEmailTemplate
            {
                Loan_Details_URL = "https://www.google.com/",
                Start_Timestamp = DateTime.Now.ToString()
            });

            return RedirectToAction("Index", "EmailTest", new { Area = "Email", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} sent email." });
        }

        [HttpGet]
        public async Task<IActionResult> SendWarrantyExpiredEmail()
        {
            await __ApplicationEmailSender.SendWarrantyExpiredEmail(TEST_EMAIL, "AIM - Warranty Expired", new WarrantyExpiredTemplate
            {
                Warranty_Expiry_URL = "https://www.google.com/"
            });

            return RedirectToAction("Index", "EmailTest", new { Area = "Email", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} sent email." });
        }

        [HttpGet]
        public async Task<IActionResult> SendWarrantyNearlyExpiredEmail()
        {
            await __ApplicationEmailSender.SendWarrantyNearlyExpiredEmail(TEST_EMAIL, "AIM - Warranty Nearly Expired", new WarrantyNearlyExpiredTemplate
            {
                Warranty_Expiry_URL = "https://www.google.com/"
            });

            return RedirectToAction("Index", "EmailTest", new { Area = "Email", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} sent email." });
        }

        [HttpGet]
        public async Task<IActionResult> SendLoanOverdueEmail()
        {
            await __ApplicationEmailSender.SendLoanOverdueEmail(TEST_EMAIL, "AIM - Loan Overdue", new LoanOverdueTemplate
            {
                Overdue_Loan_URL = "https://www.google.com/",
                Loan_Expiry_Date = DateTime.Now.ToString()
            });

            return RedirectToAction("Index", "EmailTest", new { Area = "Email", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} sent email." });
        }
    }
}
