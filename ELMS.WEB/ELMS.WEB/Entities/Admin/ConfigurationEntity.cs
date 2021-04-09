using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Configuration;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Admin
{
    public class ConfigurationEntity : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public ConfigurationType Type { get; set; }
    }
}