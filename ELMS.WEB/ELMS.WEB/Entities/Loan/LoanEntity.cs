using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Loan;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Entities.Loan
{
    public class LoanEntity : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "Untitled";

        [ForeignKey("Loaner")]
        public string LoanerUID { get; set; }

        public string LoaneeUID { get; set; }

        [Required]
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

        public IdentityUser Loaner { get; set; }
    }
}