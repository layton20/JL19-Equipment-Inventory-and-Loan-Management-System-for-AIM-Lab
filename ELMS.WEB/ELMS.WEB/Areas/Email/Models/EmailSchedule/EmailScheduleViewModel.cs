using ELMS.WEB.Enums.Email;
using System;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class EmailScheduleViewModel
    {
        public Guid UID { get; set; }
        public Guid EmailTemplateUID { get; set; }
        public string RecipientEmail { get; set; }
        public EmailType EmailType { get; set; }
        public DateTime SendTimestamp { get; set; }
        public bool EmailSent { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}