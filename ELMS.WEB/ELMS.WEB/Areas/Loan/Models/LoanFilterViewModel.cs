using ELMS.WEB.Enums.General;
using ELMS.WEB.Enums.Loan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class LoanFilterViewModel
    {
        [Display(Name = "Loaner Email")]
        public string LoanerEmail { get; set; }

        [Display(Name = "Loanee Email")]
        public string LoaneeEmail { get; set; }

        [Display(Name = "Scheduled From")]
        public DateTime FromTimestamp { get; set; }

        [Display(Name = "Scheduled To")]
        public DateTime ExpiryTimestamp { get; set; }

        public IList<Status> Statuses { get; set; }

        [Display(Name = "Accepted Terms & Conditions")]
        public BooleanFilter AcceptedTermsAndConditions { get; set; } = BooleanFilter.All;

        [Display(Name = "Loan Created From")]
        public DateTime CreatedFromTimestamp { get; set; }

        [Display(Name = "Loan Created To")]
        public DateTime CreatedToTimestamp { get; set; }
    }
}