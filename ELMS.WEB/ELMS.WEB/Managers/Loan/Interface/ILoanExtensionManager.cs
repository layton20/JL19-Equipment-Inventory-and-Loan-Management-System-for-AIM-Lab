using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Interface
{
    public interface ILoanExtensionManager
    {
        Task<LoanExtensionResponse> CreateAsync(CreateLoanExtensionRequest request);
        Task<BaseResponse> DeleteAsync(Guid uid);
        Task<BaseResponse> UpdateAsync(UpdateLoanExtensionRequest request);
        Task<LoanExtensionResponse> GetByUIDAsync(Guid uid);
        Task<IList<LoanExtensionResponse>> GetAsync(Guid loanUID);
    }
}
