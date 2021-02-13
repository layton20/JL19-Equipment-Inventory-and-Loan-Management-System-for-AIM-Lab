using ELMS.WEB.Areas.Admin.Models.Blacklist;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.CustomDataAnnotations;
using Microsoft.AspNetCore.Identity;
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
        [EmailAddress]
        public string LoaneeEmailAddress { get; set; }
        public IList<EquipmentViewModel> SelectedEquipmentList { get; set; }
        [Required]
        public IList<Guid> SelectedEquipment { get; set; }
        [Required]
        [FutureOrTodayDate]
        public DateTime FromTimestamp { get; set; }
        [Required]
        [FutureOrTodayDate]
        public DateTime ExpiryTimestamp { get; set; }
        public IList<BlacklistViewModel> Blacklists { get; set; }
    }
}
