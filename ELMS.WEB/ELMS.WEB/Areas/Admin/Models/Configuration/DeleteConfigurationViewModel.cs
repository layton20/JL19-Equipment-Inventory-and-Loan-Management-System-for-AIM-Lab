using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Configuration
{
    public class DeleteConfigurationViewModel
    {
        [Required]
        public Guid UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        [Required]
        [Display(Name = "Verification Text")]
        public string VerificationName { get; set; }
    }
}
