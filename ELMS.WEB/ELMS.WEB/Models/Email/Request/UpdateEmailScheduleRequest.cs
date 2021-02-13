using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Email.Request
{
    public class UpdateEmailScheduleRequest
    {
        [Required]
        public Guid UID { get; set; }

        [Required]
        public Guid EmailTemplateUID { get; set; }

        [Required]
        public Guid SenderUID { get; set; }

        [Required]
        [EmailAddress]
        public string RecipientEmailAddress { get; set; }

        [Required]
        public EmailScheduleStatus Status { get; set; } = EmailScheduleStatus.Scheduled;

        [Required]
        public DateTime SendTimestamp { get; set; }

        [Required]
        public DateTime CreatedTimestamp { get; set; }

        [Required]
        public DateTime AmendedTimestamp { get; set; }
    }
}