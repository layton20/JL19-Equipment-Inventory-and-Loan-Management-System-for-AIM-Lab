﻿using ELMS.WEB.Areas.Admin.Models.Blacklist;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.CustomDataAnnotations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class CreateLoanViewModel
    {
        public IList<EquipmentViewModel> EquipmentSelectList { get; set; }
        public IList<IdentityUser> UserSelectList { get; set; }
        public string LoaneeUID { get; set; } = Guid.Empty.ToString();

        [Required(ErrorMessage = "Select an existing user or manually provide an email address")]
        [Display(Name = "Loanee Email Address")]
        public string LoaneeEmailAddress { get; set; }

        public string LoanerUID { get; set; }

        [Required]
        [FutureOrTodayDate]
        [Display(Name = "Loan From")]
        public DateTime FromTimestamp { get; set; } = DateTime.Today;

        [Required]
        [FutureDate]
        [Display(Name = "Loan To")]
        public DateTime ExpiryTimestamp { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Select at least one equipment to loan off")]
        public IList<Guid> SelectedEquipment { get; set; }

        public IList<BlacklistViewModel> Blacklists { get; set; }

        public IList<DateTime> DisabledDates { get; set; } = new List<DateTime>();
    }
}