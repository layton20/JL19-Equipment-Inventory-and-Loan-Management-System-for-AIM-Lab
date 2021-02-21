using ELMS.WEB.Areas.Admin.Models.Blacklist;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class ConfirmationLoanViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string LoanerEmailAddress { get; set; }
        [Required]
        [FutureOrTodayDate]
        public DateTime FromTimestamp { get; set; }
        [Required]
        [FutureOrTodayDate]
        public DateTime ExpiryTimestamp { get; set; }
        [Required]
        [EmailAddress]
        public string LoaneeEmailAddress { get; set; }
        public IList<EquipmentViewModel> SelectedEquipmentList { get; set; }
        [Required]
        public IList<Guid> Equipment { get; set; }
        public IList<BlacklistViewModel> Blacklists { get; set; }
    }
}
