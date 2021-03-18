using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Interface
{
    public interface ILoanEquipmentManager
    {
        public Task<IList<LoanEquipmentResponse>> GetAsync();

        public Task<IList<LoanEquipmentResponse>> GetByEquipmentAsync(Guid equipmentUID);

        public Task<IList<LoanEquipmentResponse>> GetAsync(Guid loanUID);

        public Task<BaseResponse> DeleteAsync(Guid uid);
    }
}