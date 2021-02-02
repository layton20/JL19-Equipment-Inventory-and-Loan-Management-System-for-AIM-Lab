using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Loan;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Loan.Request
{
    public class UpdateLoanRequest
    {
        [Required]
        public Guid UID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "Untitled";
        [Required]
        [CurrentDateAttribute]
        public DateTime FromTimestamp { get; set; } = DateTime.Now;

        [Required]
        [CurrentDateAttribute]
        public DateTime ExpiryTimestamp { get; set; }
        [Required]
        public bool AcceptedTermsAndConditions { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
