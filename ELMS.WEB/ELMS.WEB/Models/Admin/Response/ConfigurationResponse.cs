using ELMS.WEB.Enums.Configuration;
using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Admin.Response
{
    public class ConfigurationResponse : BaseResponse
    {
        public Guid UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public ConfigurationType Type { get; set; }
    }
}