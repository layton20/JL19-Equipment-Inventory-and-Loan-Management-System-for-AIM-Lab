using AutoMapper;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Repositories.Email.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Concrete
{
    public class EmailTemplateManager : IEmailTemplateManager
    {
        private readonly IMapper __Mapper;
        private readonly IEmailTemplateRepository __EmailTemplateRepository;
        private const string MODEL_NAME = "Email template";

        public EmailTemplateManager(IMapper mapper, IEmailTemplateRepository emailTemplateRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EmailTemplateRepository = emailTemplateRepository ?? throw new ArgumentNullException(nameof(emailTemplateRepository));
        }

        public async Task<EmailTemplateResponse> CreateAsync(CreateEmailTemplateRequest request)
        {
            EmailTemplateResponse _Response = __Mapper.Map<EmailTemplateResponse>(await __EmailTemplateRepository.CreateAsync(__Mapper.Map<EmailTemplateEntity>(request)));

            if (_Response == null)
            {
                _Response = new EmailTemplateResponse();
                _Response.Success = false;
                _Response.Message = $"Error: {GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailTemplateRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"Error: {GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EmailTemplatesResponse> GetAsync()
        {
            EmailTemplatesResponse _Response = new EmailTemplatesResponse
            {
                EmailTemplates = __Mapper.Map<IList<EmailTemplateResponse>>(await __EmailTemplateRepository.GetAsync())
            };

            if (_Response.EmailTemplates == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} get ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EmailTemplateResponse> GetByUIDAsync(Guid uid)
        {
            EmailTemplateResponse _Response = __Mapper.Map<EmailTemplateResponse>(await __EmailTemplateRepository.GetByUIDAsync(uid));

            if (_Response == null)
            {
                return new EmailTemplateResponse
                {
                    Success = false,
                    Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} get ${MODEL_NAME}."
                };
            }

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(UpdateEmailTemplateRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty || !await __EmailTemplateRepository.UpdateAsync(__Mapper.Map<EmailTemplateEntity>(request)))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} update ${MODEL_NAME}.";
            }

            return _Response;
        }
    }
}