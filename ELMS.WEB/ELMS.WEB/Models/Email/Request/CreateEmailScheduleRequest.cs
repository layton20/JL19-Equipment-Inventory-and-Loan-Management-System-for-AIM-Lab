using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Email.Request
{
    public class CreateEmailScheduleRequest
    {
        [Required]
        public Guid EmailTemplateUID { get; set; }

        [Required]
        [EmailAddress]
        public string RecipientEmailAddress { get; set; }

        [Required]
        public DateTime SendTimestamp { get; set; }
    }
}
