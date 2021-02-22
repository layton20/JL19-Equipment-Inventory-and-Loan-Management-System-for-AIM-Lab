using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class UpdateLoanViewModel
    {
        [Required]
        public Guid UID { get; set; }

        [Required]
        public bool AcceptedTermsAndConditions { get; set; }

        [Required]
        public DateTime StartTimestamp { get; set; }

        [Required]
        public DateTime ExpiryTimestamp { get; set; }
    }
}