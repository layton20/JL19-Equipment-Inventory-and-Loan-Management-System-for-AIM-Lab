using ELMS.WEB.Enums.Email;
using ELMS.WEB.Enums.General;
using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Email.Response
{
    public class EmailScheduleResponse : BaseEntityResponse
    {
        public Guid EmailTemplateUID { get; set; }
        public string RecipientEmailAddress { get; set; }
        public EmailType EmailType { get; set; }
        public DateTime SendTimestamp { get; set; }
        public bool Sent { get; set; }
    }
}