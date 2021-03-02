using ELMS.WEB.Entities.Loan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Loan.Interface
{
    public interface ILoanExtensionRepository
    {
        Task<LoanExtensionEntity> CreateAsync(LoanExtensionEntity entity);

        Task<bool> DeleteAsync(Guid uid);

        Task<bool> UpdateAsync(LoanExtensionEntity entity);

        Task<LoanExtensionEntity> GetByUIDAsync(Guid uid);

        Task<IList<LoanExtensionEntity>> GetAsync(Guid loanUid);
    }
}