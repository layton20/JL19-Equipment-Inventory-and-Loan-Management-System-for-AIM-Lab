using System;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class AlreadyAcceptedTermsAndConditionsViewModel
    {
        public Guid UID { get; set; }
        public bool Accepted { get; set; } = false;
        public LoanViewModel Loan { get; set; }
    }
}
