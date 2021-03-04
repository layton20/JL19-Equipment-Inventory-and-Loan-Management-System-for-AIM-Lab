using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Configuration
{
    public class UpdateConfigurationViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
