using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;
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
        public Task<BaseResponse> CreateEquipmentWarrantyScheduleAsync(EquipmentResponse equipment, string baseURL);
        public Task<BaseResponse> BulkCreateEquipmentWarrantyScheduleAsync(IList<EquipmentResponse> equipmentList, string baseURL);
        public Task<BaseResponse> CreateLoanConfirmScheduleAsync(LoanResponse loan, string baseURL, bool forceSend = false);
        public Task<BaseResponse> CreateLoanConfirmedScheduleAsync(LoanResponse loan, string baseURL, bool forceSend = false);
        public Task<BaseResponse> CreateLoanExpiryScheduleAsync(LoanResponse loan, string baseURL, bool forceSend = false);
        public Task SendScheduledEmails();
    }
}
