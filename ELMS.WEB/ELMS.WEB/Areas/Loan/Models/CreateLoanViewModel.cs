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
        [Required]
        [MaxLength(50)]
        [Display(Name = "Loan Name")]
        public string Name { get; set; }

        public IList<EquipmentViewModel> EquipmentSelectList { get; set; }
        public IList<IdentityUser> UserSelectList { get; set; }
        public string LoaneeUID { get; set; } = Guid.Empty.ToString();

        [Required(ErrorMessage = "Select an existing user or manually provide an email address")]
        [Display(Name = "Loanee Email Address")]
        public string LoaneeEmailAddress { get; set; }

        public string LoanerUID { get; set; }

        [Required]
        [FutureOrTodayDate]
        [Display(Name = "From Date")]
        public DateTime FromTimestamp { get; set; } = DateTime.Today;

        [Required]
        [FutureDate]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryTimestamp { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Select at least one equipment to loan off")]
        public IList<Guid> SelectedEquipment { get; set; }
    }
}