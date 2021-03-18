using AutoMapper;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
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
        private readonly string MODEL_NAME = "Loan equipment";

        public LoanEquipmentManager(IMapper mapper, ILoanEquipmentRepository loanEquipmentRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanEquipmentRepository = loanEquipmentRepository ?? throw new ArgumentNullException(nameof(loanEquipmentRepository));
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __LoanEquipmentRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
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

        public async Task<IList<LoanEquipmentResponse>> GetAsync()
        {
            return __Mapper.Map<IList<LoanEquipmentResponse>>(await __LoanEquipmentRepository.GetAsync());
        }

        public async Task<IList<LoanEquipmentResponse>> GetByEquipmentAsync(Guid equipmentUID)
        {
            return __Mapper.Map<IList<LoanEquipmentResponse>>(await __LoanEquipmentRepository.GetByEquipmentAsync(equipmentUID));
        }
    }
}