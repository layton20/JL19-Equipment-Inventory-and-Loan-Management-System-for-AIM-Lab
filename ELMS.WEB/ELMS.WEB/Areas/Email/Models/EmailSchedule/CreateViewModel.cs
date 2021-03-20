using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.CustomDataAnnotations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class CreateViewModel
    {
        [Required]
        [NonDefaultGuid]
        [Display(Name = "Email Template")]
        public Guid SelectedEmailTemplate { get; set; }

        [Required(ErrorMessage = "Recipient emails must be provided.")]
        [Display(Name = "Recipients")]
        public IList<string> SelectedRecipientEmailAddresses { get; set; }

        [Required]
        [FutureOrTodayDate]
        [Display(Name = "Schedule Time")]
        public DateTime SendTimestamp { get; set; } = DateTime.Today;

        public IList<EmailTemplateViewModel> EmailTemplates { get; set; } = new List<EmailTemplateViewModel>();

        public IList<IdentityUser> Users { get; set; } = new List<IdentityUser>();
    }
}