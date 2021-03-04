using System;

namespace ELMS.WEB.Models.Admin.Request
{
    public class UpdateConfigurationRequest
    {
        public Guid UID { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
