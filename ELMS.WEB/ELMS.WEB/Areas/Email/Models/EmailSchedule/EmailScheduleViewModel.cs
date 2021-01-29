using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.Enums.Email;
using System;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class EmailScheduleViewModel
    {
        public Guid UID { get; set; }
        public Guid EmailTemplateUID { get; set; }
        public Guid SenderUID { get; set; }
        public string RecipientEmailAddress { get; set; }
        public EmailScheduleStatus Status { get; set; } = EmailScheduleStatus.Scheduled;
        public DateTime SendTimestamp { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
        public EmailTemplateViewModel EmailTemplate { get; set; }
    }
}
