using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Email
{
    public class EmailScheduleEntity : BaseEntity
    {
        [Required]
        public Guid EmailTemplateUID { get; set; }

        [Required]
        [EmailAddress]
        public string RecipientEmailAddress { get; set; }
        [Required]
        public EmailScheduleStatus Status { get; set; } = EmailScheduleStatus.Scheduled;

        [Required]
        public DateTime SendTimestamp { get; set; }
    }
}