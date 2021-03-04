using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Configuration;

namespace ELMS.WEB.Entities.Admin
{
    public class ConfigurationEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ConfigurationType Type { get; set; }
    }
}
