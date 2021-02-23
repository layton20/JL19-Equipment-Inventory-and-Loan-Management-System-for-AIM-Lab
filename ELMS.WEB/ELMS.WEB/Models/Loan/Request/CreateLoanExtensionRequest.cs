using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Models.Loan.Request
{
    public class CreateLoanExtensionRequest
    {
        [Required]
        public Guid LoanUID { get; set; }
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
