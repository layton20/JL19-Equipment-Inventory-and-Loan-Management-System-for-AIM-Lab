using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Email.Response
{
    public class EmailScheduleParameterResponse : BaseEntityResponse
    {
        public Guid EmailScheduleUID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}