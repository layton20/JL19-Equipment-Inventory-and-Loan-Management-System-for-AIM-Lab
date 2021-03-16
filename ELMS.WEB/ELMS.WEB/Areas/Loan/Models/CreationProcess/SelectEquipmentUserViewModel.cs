using ELMS.WEB.Areas.Admin.Models.Blacklist;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.CustomDataAnnotations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models.CreationProcess
{
    public class SelectEquipmentUserViewModel
    {
        public IList<EquipmentViewModel> EquipmentSelectList { get; set; } = new List<EquipmentViewModel>();

        public IList<IdentityUser> UserSelectList { get; set; } = new List<IdentityUser>();
        public IList<BlacklistViewModel> Blacklists { get; set; } = new List<BlacklistViewModel>();

        [Required]
        public IList<Guid> Equipment { get; set; }

        [Required(ErrorMessage = "Select an existing user or manually provide an email address")]
        [Display(Name = "Loanee Email Address")]
        public string LoaneeEmailAddress { get; set; }
    }
}
