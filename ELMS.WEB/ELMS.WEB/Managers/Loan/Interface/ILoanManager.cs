using ELMS.WEB.Enums.Loan;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Interface
{
    public interface ILoanManager
    {
        public Task<LoanResponse> CreateAsync(CreateLoanRequest loan);

        public Task<LoanResponse> GetByUIDAsync(Guid uid);
        public Task<IList<LoanResponse>> GetAsync();

        public Task<IList<LoanResponse>> GetAsync(Guid equipmentUID, bool all = false);

        public Task<BaseResponse> AcceptTermsAndConditions(Guid uid);

        public Task<BaseResponse> ChangeStatusAsync(Guid uid, Status status);

        public Task<BaseResponse> UpdateAsync(UpdateLoanRequest loan);
        public Task<IntResponse> GetCountByStatus(Status status);
        public Task<IList<LoanResponse>> GetByUserAsync(string loaneeEmail);
    }
}
