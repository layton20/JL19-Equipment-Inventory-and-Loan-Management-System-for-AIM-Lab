using ELMS.WEB.Adapters.Email;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Repositories.Email.Interface;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Concrete
{
    public class EmailScheduleManager : IEmailScheduleManager
    {
        private readonly IEmailScheduleRepository __EmailScheduleRepository;
        private const string MODEL_NAME = "Email schedule";

        public EmailScheduleManager(IEmailScheduleRepository emailScheduleRepository)
        {
            __EmailScheduleRepository = emailScheduleRepository ?? throw new ArgumentNullException(nameof(emailScheduleRepository));
        }

        public async Task<EmailScheduleResponse> CreateAsync(CreateEmailScheduleRequest request)
        {
            EmailScheduleResponse _Response = (await __EmailScheduleRepository.CreateAsync(request.ToEntity())).ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} create ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailScheduleRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} delete ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EmailSchedulesResponse> GetAsync()
        {
            EmailSchedulesResponse _Response = new EmailSchedulesResponse
            {
                Responses = (await __EmailScheduleRepository.GetAsync()).ToResponse()
            };

            if (_Response.Responses == null || _Response.Responses.Count <= 0)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} get ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(UpdateEmailScheduleRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty && !await __EmailScheduleRepository.UpdateAsync(request.ToEntity()))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} update ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EmailScheduleResponse> GetByUIDAsync(Guid uid)
        {
            EmailScheduleResponse _Response = (await __EmailScheduleRepository.GetByUIDAsync(uid)).ToResponse();

            if (_Response == null)
            {
                return new EmailScheduleResponse
                {
                    Success = false,
                    Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} get ${MODEL_NAME}."
                };
            }

            return _Response;
        }
    }
}