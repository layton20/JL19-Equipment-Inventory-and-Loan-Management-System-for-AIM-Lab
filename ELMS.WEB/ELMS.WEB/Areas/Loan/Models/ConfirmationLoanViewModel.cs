using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class ConfirmationLoanViewModel
    {
        [Required]
        [EmailAddress]
        public string LoanerEmail { get; set; }

        [Required]
        [FutureOrTodayDate]
        [DateLessThan("ExpiryTimestamp")]
        public DateTime FromTimestamp { get; set; }

        [Required]
        [FutureOrTodayDate]
        public DateTime ExpiryTimestamp { get; set; }

        [Required]
        [EmailAddress]
        public string LoaneeEmail { get; set; }

        public IList<EquipmentViewModel> SelectedEquipmentList { get; set; }

        [Required]
        public IList<Guid> Equipment { get; set; }

        public BlacklistStateEnum BlacklistState { get; set; } = BlacklistStateEnum.None;
    }
}