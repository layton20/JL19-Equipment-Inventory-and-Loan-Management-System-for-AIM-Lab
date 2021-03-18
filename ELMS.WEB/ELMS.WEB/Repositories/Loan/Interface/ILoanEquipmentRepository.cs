using ELMS.WEB.Entities.Loan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Loan.Interface
{
    public interface ILoanEquipmentRepository
    {
        public Task<IList<LoanEquipmentEntity>> GetAsync();

        public Task<IList<LoanEquipmentEntity>> GetByEquipmentAsync(Guid equipmentUID);

        public Task<IList<LoanEquipmentEntity>> GetAsync(Guid loanUID);

        public Task<bool> DeleteAsync(Guid uid);
    }
}