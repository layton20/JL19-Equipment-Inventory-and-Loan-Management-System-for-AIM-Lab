using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Interface
{
    public interface IEmailScheduleManager
    {
        public Task<EmailScheduleResponse> CreateAsync(CreateEmailScheduleRequest request);
        public Task<IList<EmailScheduleResponse>> GetAsync();
        public Task<EmailScheduleResponse> GetByUIDAsync(Guid uid);
        public Task<BaseResponse> DeleteAsync(Guid uid);
    }
}
