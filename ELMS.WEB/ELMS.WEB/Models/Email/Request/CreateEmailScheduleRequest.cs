using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Email.Request
{
    public class CreateEmailScheduleRequest
    {
        public Guid EmailTemplateUID { get; set; } = Guid.Empty;

        [Required]
        [EmailAddress]
        public string RecipientEmailAddress { get; set; }

        [Required]
        public EmailType EmailType { get; set; }

        [Required]
        public bool Sent { get; set; } = false;

        [Required]
        public DateTime SendTimestamp { get; set; }
    }
}