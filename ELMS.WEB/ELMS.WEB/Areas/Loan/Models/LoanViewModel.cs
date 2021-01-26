using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Enums.Loan;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class LoanViewModel
    {
        public Guid UID { get; set; }
        public string Name { get; set; }
        public IdentityUser Loaner { get; set; }
        public IdentityUser Loanee { get; set; }
        public string LoaneeEmail { get; set; }
        public DateTime FromTimestamp { get; set; }
        public DateTime ExpiryTimestamp { get; set; }
        public Status Status { get; set; }
        public bool AcceptedTermsAndConditions { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
        public IList<EquipmentViewModel> EquipmentList { get; set; }
    }
}
