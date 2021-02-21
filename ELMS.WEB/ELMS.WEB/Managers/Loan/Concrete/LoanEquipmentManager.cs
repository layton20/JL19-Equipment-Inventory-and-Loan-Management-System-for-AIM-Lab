using AutoMapper;
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
        private readonly IMapper __Mapper;
        private readonly ILoanEquipmentRepository __LoanEquipmentRepository;

        public LoanEquipmentManager(IMapper mapper, ILoanEquipmentRepository loanEquipmentRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanEquipmentRepository = loanEquipmentRepository ?? throw new ArgumentNullException(nameof(loanEquipmentRepository));
        }

        public async Task<IList<LoanEquipmentResponse>> GetAsync(Guid loanUID)
        {
            IList<LoanEquipmentResponse> _Responses = __Mapper.Map<IList<LoanEquipmentResponse>>(await __LoanEquipmentRepository.GetAsync(loanUID));

            if (_Responses == null)
            {
                return Enumerable.Empty<LoanEquipmentResponse>().ToList();
            }

            return _Responses;
        }
    }
}