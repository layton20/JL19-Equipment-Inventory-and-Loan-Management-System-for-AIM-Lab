using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Loan.Request
{
    public class UpdateLoanExtensionRequest
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