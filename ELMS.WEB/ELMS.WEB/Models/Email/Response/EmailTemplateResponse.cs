using ELMS.WEB.Enums.Email;
using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Email.Response
{
    public class EmailTemplateResponse : BaseEntityResponse
    {
        public string Name { get; set; }
        public EmailTemplateType TemplateType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid OwnerUID { get; set; }
    }
}