using ELMS.WEB.Models.Base.Response;
using System.Collections.Generic;

namespace ELMS.WEB.Models.Email.Response
{
    public class EmailTemplatesResponse : BaseResponse
    {
        public IList<EmailTemplateResponse> EmailTemplates { get; set; }
    }
}