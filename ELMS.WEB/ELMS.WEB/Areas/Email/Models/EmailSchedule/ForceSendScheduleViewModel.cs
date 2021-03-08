using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class ForceSendScheduleViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }

        public string RecipientEmailAddress { get; set; }

        [Display(Name = "Retain Schedule")]
        public bool RetainSchedule { get; set; }

        [Display(Name = "Send me an email copy")]
        public bool SendCopyToSelf { get; set; }
    }
}