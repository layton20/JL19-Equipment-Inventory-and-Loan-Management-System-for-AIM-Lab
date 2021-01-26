using ELMS.WEB.Models.Loan.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Interface
{
    public interface ILoanEquipmentManager
    {
        public Task<IList<LoanEquipmentResponse>> GetAsync(Guid loanUID);
    }
}
