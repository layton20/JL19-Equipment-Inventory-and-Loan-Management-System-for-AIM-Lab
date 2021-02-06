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
    public class EmailScheduleParameterManager : IEmailScheduleParameterManager
    {
        private readonly IEmailScheduleParameterRepository __EmailScheduleParameterRepository;
        private const string MODEL_NAME = "Email schedule parameter";

        public EmailScheduleParameterManager(IEmailScheduleParameterRepository emailScheduleParameterRepository)
        {
            __EmailScheduleParameterRepository = emailScheduleParameterRepository ?? throw new ArgumentNullException(nameof(emailScheduleParameterRepository));
        }

        public async Task<EmailScheduleParameterResponse> CreateAsync(CreateEmailScheduleParameterRequest request)
        {
            EmailScheduleParameterResponse _Response = (await __EmailScheduleParameterRepository.CreateAsync(request.ToEntity())).ToResponse();

            if (_Response == null)
            {
                _Response = new EmailScheduleParameterResponse();
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid scheduleUID)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailScheduleParameterRepository.DeleteAsync(scheduleUID))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<EmailScheduleParameterResponse>> GetAsync(Guid scheduleUID)
        {
            if (scheduleUID == Guid.Empty)
            {
                return null;
            }

            return (await __EmailScheduleParameterRepository.GetAsync(scheduleUID)).ToResponse();
        }
    }
}
