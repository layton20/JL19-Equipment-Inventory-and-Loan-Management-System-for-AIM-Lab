using ELMS.WEB.Adapters.Email;
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
    public class EmailScheduleManager : IEmailScheduleManager
    {
        private readonly IEmailScheduleRepository __EmailScheduleRepository;
        private const string MODEL_NAME = "Email Schedule";

        public EmailScheduleManager(IEmailScheduleRepository emailScheduleRepository)
        {
            __EmailScheduleRepository = emailScheduleRepository ?? throw new ArgumentNullException(nameof(emailScheduleRepository));
        }

        public async Task<EmailScheduleResponse> CreateAsync(CreateEmailScheduleRequest request)
        {
            EmailScheduleResponse _Response = (await __EmailScheduleRepository.CreateAsync(request?.ToEntity()))?.ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<EmailScheduleResponse>> GetAsync()
        {
            return (await __EmailScheduleRepository.GetAsync()).ToResponse();
        }

        public async Task<EmailScheduleResponse> GetByUIDAsync(Guid uid)
        {
            return (await __EmailScheduleRepository.GetByUIDAsync(uid)).ToResponse();
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailScheduleRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }
    }
}