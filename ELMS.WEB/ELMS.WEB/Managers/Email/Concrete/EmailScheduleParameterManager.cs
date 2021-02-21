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
    public class EmailScheduleParameterManager : IEmailScheduleParameterManager
    {
        private readonly IMapper __Mapper;
        private readonly IEmailScheduleParameterRepository __EmailScheduleParameterRepository;
        private const string MODEL_NAME = "Email schedule parameter";

        public EmailScheduleParameterManager(IMapper mapper, IEmailScheduleParameterRepository emailScheduleParameterRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EmailScheduleParameterRepository = emailScheduleParameterRepository ?? throw new ArgumentNullException(nameof(emailScheduleParameterRepository));
        }

        public async Task<EmailScheduleParameterResponse> CreateAsync(CreateEmailScheduleParameterRequest request)
        {
            EmailScheduleParameterResponse _Response = __Mapper.Map<EmailScheduleParameterResponse>(await __EmailScheduleParameterRepository.CreateAsync(__Mapper.Map<EmailScheduleParameterEntity>(request)));

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

            return __Mapper.Map<IList<EmailScheduleParameterResponse>>(await __EmailScheduleParameterRepository.GetAsync(scheduleUID));
        }
    }
}