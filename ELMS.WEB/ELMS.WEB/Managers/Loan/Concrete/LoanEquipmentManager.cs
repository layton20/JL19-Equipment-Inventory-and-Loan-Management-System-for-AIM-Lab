using ELMS.WEB.Adapters.Loan;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Loan.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Concrete
{
    public class LoanEquipmentManager : ILoanEquipmentManager
    {
        private readonly ILoanEquipmentRepository __LoanEquipmentRepository;

        public LoanEquipmentManager(ILoanEquipmentRepository loanEquipmentRepository)
        {
            __LoanEquipmentRepository = loanEquipmentRepository ?? throw new ArgumentNullException(nameof(loanEquipmentRepository));
        }

        public async Task<IList<LoanEquipmentResponse>> GetAsync(Guid loanUID)
        {
            IList<LoanEquipmentResponse> _Responses = (await __LoanEquipmentRepository.GetAsync(loanUID)).ToResponse();

            if (_Responses == null)
            {
                return Enumerable.Empty<LoanEquipmentResponse>().ToList();
            }

            return _Responses;
        }
    }
}
