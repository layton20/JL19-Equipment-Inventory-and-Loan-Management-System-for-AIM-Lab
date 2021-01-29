using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class CreateEmailScheduleViewModel
    {
        public IList<SelectListItem> EmailTemplates { get; set; }
        [Required]
        [Display(Name = "Email Template")]
        public Guid SelectedTemplateUID { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Recipent Email Address")]
        public string RecipientEmailAddress { get; set; }
        [Required]
        [Display(Name = "Schedule Time")]
        public DateTime ScheduleTime { get; set; }
    }
}
