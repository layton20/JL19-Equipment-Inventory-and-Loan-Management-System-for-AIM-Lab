using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Loan;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class ForceCompleteLoanViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }
        [Display(Name = "Loan Name")]
        public string Name { get; set; }
        public string LoaneeEmail { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartTimestamp { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryTimestamp { get; set; }
    }
}
