using ELMS.WEB.Enums.Configuration;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Configuration
{
    public class CreateConfigurationViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public ConfigurationType Type { get; set; }
    }
}