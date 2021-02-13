using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Loan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Loan.Request
{
    public class CreateLoanRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "Untitled";

        [Required]
        [EmailAddress]
        public string LoanerEmailAddress { get; set; }

        [Required]
        [EmailAddress]
        public string LoaneeEmailAddress { get; set; }

        [Required]
        [FutureOrTodayDate]
        public DateTime FromTimestamp { get; set; } = DateTime.Now;

        [Required]
        [FutureDate]
        public DateTime ExpiryTimestamp { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Pending;

        [Required]
        public bool AcceptedTermsAndConditions { get; set; } = false;

        [Required]
        public IList<Guid> EquipmentList { get; set; }
    }
}