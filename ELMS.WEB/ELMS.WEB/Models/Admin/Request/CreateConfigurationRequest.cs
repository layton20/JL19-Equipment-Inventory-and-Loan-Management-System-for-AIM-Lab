using ELMS.WEB.Enums.Configuration;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Admin.Request
{
    public class CreateConfigurationRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public ConfigurationType Type { get; set; }
    }
}
