using ELMS.WEB.Enums.Email;
using ELMS.WEB.Enums.General;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class EmailScheduleFilterViewModel
    {
        public IList<SelectListItem> EmailTemplatesSelectList { get; set; } = new List<SelectListItem>();
        [Display(Name = "Email Template(s)")]
        public IList<Guid> EmailTemplateUIs { get; set; } = new List<Guid>();
        [Display(Name = "Recipient Email")]
        public string RecipientEmailAddress { get; set; }
        [Display(Name = "Email Type(s)")]
        public IList<EmailType> EmailTypes { get; set; }
        [Display(Name = "Email Sent Status")]
        public BooleanFilter EmailSent { get; set; }
        [Display(Name = "Email Scheduled From")]
        public DateTime ScheduledForFrom { get; set; } = DateTime.MinValue;
        [Display(Name = "Email Scheduled To")]
        public DateTime ScheduledForTo { get; set; } = DateTime.MaxValue;
    }
}
