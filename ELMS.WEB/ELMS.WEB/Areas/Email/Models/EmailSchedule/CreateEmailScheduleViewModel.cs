using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Email;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class CreateEmailScheduleViewModel
    {
        [Required]
        public Guid EmailTemplateUID { get; set; } = Guid.Empty;

        [Required]
        [Display(Name = "Recipient Email")]
        public string RecipientEmailAddress { get; set; }

        [Required]
        public EmailType EmailType { get; set; }

        [Required]
        [FutureOrTodayDate]
        public DateTime SendTimestamp { get; set; }

        public IList<EmailTemplateViewModel> EmailTemplates { get; set; }
    }
}