using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models.LoanExtension
{
    public class CreateLoanExtensionViewModel
    {
        [Required]
        [Display(Name = "Loan UID")]
        public Guid LoanUID { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Extender Email")]
        public string ExtenderEmail { get; set; }
        [Required]
        [Display(Name = "Previous Expiry Date")]
        public DateTime PreviousExpiryDate { get; set; }
        [Required]
        [FutureOrTodayDate]
        [Display(Name = "New Expiry Date")]
        public DateTime NewExpiryDate { get; set; }
    }
}
