using ELMS.WEB.Enums.Configuration;
using System;

namespace ELMS.WEB.Areas.Admin.Models.Configuration
{
    public class ConfigurationViewModel
    {
        public Guid UID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ConfigurationType Type { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}
