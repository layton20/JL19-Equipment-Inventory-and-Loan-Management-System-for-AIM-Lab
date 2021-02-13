using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class CreateCustomEmailScheduleViewModel
    {
        [Required]
        [NonDefaultGuid]
        [Display(Name = "Email Template")]
        public Guid EmailTemplateUID { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Recipient Email")]
        public string RecipientEmailAddress { get; set; }

        [Required]
        [FutureOrTodayDate]
        [Display(Name = "Scheduled For")]
        public DateTime SendTimestamp { get; set; } = DateTime.Today.Date.AddHours(12);

        public IList<EmailTemplateViewModel> EmailTemplates { get; set; }
    }
}