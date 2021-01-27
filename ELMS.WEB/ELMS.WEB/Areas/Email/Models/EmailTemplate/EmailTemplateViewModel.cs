using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailTemplate
{
    public class EmailTemplateViewModel
    {
        [Required]
        public Guid UID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public EmailTemplateType TemplateType { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public Guid OwnerUID { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}
