using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Loan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Loan.Interface
{
    public interface ILoanRepository
    {
        public Task<LoanEntity> CreateAsync(LoanEntity loan, IList<Guid> equipmentList);

        public Task<LoanEntity> GetByUIDAsync(Guid uid);

        public Task<IList<LoanEntity>> GetAsync(Guid equipmentUID, bool all = false);

        public Task<bool> AcceptTermsAndConditions(Guid uid);

        public Task<bool> ChangeStatusAsync(Guid uid, Status status);

        public Task<bool> UpdateAsync(LoanEntity loan, IList<Guid> equipmentList);
    }
}