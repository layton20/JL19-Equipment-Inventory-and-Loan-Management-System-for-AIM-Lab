using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Email
{
    public class EmailScheduleEntity : BaseEntity
    {
        public Guid EmailTemplateUID { get; set; }

        [Required]
        [EmailAddress]
        public string RecipientEmailAddress { get; set; }

        [Required]
        public EmailType EmailType { get; set; }

        [Required]
        public DateTime SendTimestamp { get; set; }
        [Required]
        public bool Sent { get; set; } = false;
    }
}