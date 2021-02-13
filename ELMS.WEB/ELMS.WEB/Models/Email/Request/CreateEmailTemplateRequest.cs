using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Email.Request
{
    public class CreateEmailTemplateRequest
    {
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
    }
}