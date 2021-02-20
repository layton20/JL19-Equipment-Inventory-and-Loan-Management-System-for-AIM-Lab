using ELMS.WEB.Areas.Report.Data;
using ELMS.WEB.Enums.Loan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Report.Models
{
    public class LoanHistoryReportFilterViewModel
    {
        public string Name { get; set; }
        [Display(Name = "Loaner Email")]
        public string LoanerEmail { get; set; }
        [Display(Name = "Loanee Email")]
        public string LoaneeEmail { get; set; }
        [Display(Name = "Scheduled From")]
        public DateTime FromTimestamp { get; set; } = DateTime.MinValue;
        [Display(Name = "Scheduled To")]
        public DateTime ExpiryTimestamp { get; set; } = DateTime.Today.AddDays(1);
        public IList<Status> Statuses { get; set; }
        [Display(Name = "Accepted Terms & Conditions")]
        public BooleanFilter AcceptedTermsAndConditions { get; set; } = BooleanFilter.All;
        [Display(Name = "Loan Created From")]
        public DateTime CreatedFromTimestamp { get; set; } = DateTime.MinValue;
        [Display(Name = "Loan Created To")]
        public DateTime CreatedToTimestamp { get; set; } = DateTime.Today.AddDays(1);
    }
}
