using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Loan;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Loan
{
    public class LoanEntity : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "Untitled";

        [Required]
        [EmailAddress]
        public string LoanerEmail { get; set; }

        [Required]
        [EmailAddress]
        public string LoaneeEmail { get; set; }

        [Required]
        [CurrentDate]
        public DateTime FromTimestamp { get; set; } = DateTime.Now;

        [Required]
        [CurrentDate]
        public DateTime ExpiryTimestamp { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Pending;

        [Required]
        public bool AcceptedTermsAndConditions { get; set; } = false;
    }
}