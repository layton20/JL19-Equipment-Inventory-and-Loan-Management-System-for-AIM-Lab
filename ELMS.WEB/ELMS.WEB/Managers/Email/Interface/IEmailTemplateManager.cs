using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Interface
{
    public interface IEmailTemplateManager
    {
        public Task<EmailTemplateResponse> CreateAsync(CreateEmailTemplateRequest request);

        public Task<EmailTemplatesResponse> GetAsync();

        public Task<EmailTemplateResponse> GetByUIDAsync(Guid uid);

        public Task<BaseResponse> UpdateAsync(UpdateEmailTemplateRequest request);

        public Task<BaseResponse> DeleteAsync(Guid uid);
    }
}