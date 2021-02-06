using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Interface
{
    public interface IEmailScheduleParameterManager
    {
        public Task<EmailScheduleParameterResponse> CreateAsync(CreateEmailScheduleParameterRequest request);
        public Task<IList<EmailScheduleParameterResponse>> GetAsync(Guid scheduleUID);
        public Task<BaseResponse> DeleteAsync(Guid scheduleUID);
    }
}
