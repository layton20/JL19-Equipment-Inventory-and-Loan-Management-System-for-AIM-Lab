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

        [Required]
        [Display(Name = "Email Body")]
        public string Body { get; set; }

        public string OwnerUID { get; set; }
    }
}