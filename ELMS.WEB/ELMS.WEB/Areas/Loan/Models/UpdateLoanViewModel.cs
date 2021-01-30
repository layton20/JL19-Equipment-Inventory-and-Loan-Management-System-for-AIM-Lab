using ELMS.WEB.Enums.Loan;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class UpdateLoanViewModel
    {
        [Required]
        public Guid UID { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public bool TermsAndConditionsAccepted { get; set; }
    }
}
