using ELMS.WEB.Enums.Email;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailTemplate
{
    public class CreateEmailTemplateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email Format")]
        public EmailTemplateType TemplateType { get; set; }

        [Required]
        [Display(Name = "Email Subject")]
        public string Subject { get; set; }

        [Display(Name = "Email Header")]
        public string Header { get; set; }

        [Display(Name = "Email Subheader")]
        public string Subheader { get; set; }

        [Required]
        [Display(Name = "Email Body")]
        public string Body { get; set; }

        [Display(Name = "Email Footer")]
        public string Footer { get; set; }

        public string OwnerUID { get; set; }
    }
}