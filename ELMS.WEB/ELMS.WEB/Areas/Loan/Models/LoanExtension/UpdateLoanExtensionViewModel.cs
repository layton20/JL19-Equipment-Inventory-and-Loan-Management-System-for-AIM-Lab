using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models.LoanExtension
{
    public class UpdateLoanExtensionViewModel
    {
        [NonDefaultGuid]
        [Required]
        public Guid UID { get; set; }
        [Required]
        [EmailAddress]
        public string ExtenderEmail { get; set; }
        [Required]
        public DateTime PreviousExpiryDate { get; set; }
        [Required]
        [FutureOrTodayDate]
        public DateTime NewExpiryDate { get; set; }
    }
}
