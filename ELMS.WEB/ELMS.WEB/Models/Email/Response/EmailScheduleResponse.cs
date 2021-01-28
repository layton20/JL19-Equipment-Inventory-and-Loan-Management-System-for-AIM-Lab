using ELMS.WEB.Enums.Email;
using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Email.Response
{
    public class EmailScheduleResponse : BaseEntityResponse
    {
        public Guid EmailTemplateUID { get; set; }
        public Guid SenderUID { get; set; }
        public string RecipientEmailAddress { get; set; }
        public EmailScheduleStatus Status { get; set; } = EmailScheduleStatus.Scheduled;
        public DateTime SendTimestamp { get; set; }
    }
}
