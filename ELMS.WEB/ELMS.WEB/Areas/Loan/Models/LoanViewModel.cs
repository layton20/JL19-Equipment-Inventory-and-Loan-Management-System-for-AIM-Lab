using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Enums.Loan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class LoanViewModel
    {
        public Guid UID { get; set; }

        public string LoanerEmail { get; set; }
        public string LoaneeEmail { get; set; }

        [Display(Name = "Start Date")]
        public DateTime FromTimestamp { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryTimestamp { get; set; }

        public Status Status { get; set; }
        public bool AcceptedTermsAndConditions { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
        public IList<EquipmentViewModel> EquipmentList { get; set; }
    }
}