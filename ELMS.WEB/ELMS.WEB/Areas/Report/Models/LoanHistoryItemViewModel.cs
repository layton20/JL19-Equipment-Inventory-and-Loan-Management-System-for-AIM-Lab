using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.Loan.Models.LoanExtension;
using ELMS.WEB.Enums.Loan;
using System;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Report.Models
{
    public class LoanHistoryItemViewModel
    {
        public Guid UID { get; set; }
        public string LoanerEmail { get; set; }
        public string LoaneeEmail { get; set; }
        public DateTime FromTimestamp { get; set; } = DateTime.Now;
        public DateTime ExpiryTimestamp { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public bool AcceptedTermsAndConditions { get; set; } = false;
        public IList<EquipmentViewModel> EquipmentList { get; set; } = new List<EquipmentViewModel>();
        public IList<LoanExtensionViewModel> Extensions { get; set; } = new List<LoanExtensionViewModel>();
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}