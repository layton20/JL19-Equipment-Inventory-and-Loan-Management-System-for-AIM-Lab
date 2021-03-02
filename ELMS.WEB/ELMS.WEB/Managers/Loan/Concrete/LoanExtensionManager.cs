using AutoMapper;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Loan.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Concrete
{
    public class LoanExtensionManager : ILoanExtensionManager
    {
        private readonly IMapper __Mapper;
        private readonly ILoanExtensionRepository __LoanExtensionRepository;
        private const string MODEL_NAME = "Loan extension";

        public LoanExtensionManager(IMapper mapper, ILoanExtensionRepository loanExtensionRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanExtensionRepository = loanExtensionRepository ?? throw new ArgumentNullException(nameof(loanExtensionRepository));
        }

        public async Task<LoanExtensionResponse> CreateAsync(CreateLoanExtensionRequest request)
        {
            LoanExtensionResponse _Response = __Mapper.Map<LoanExtensionResponse>(await __LoanExtensionRepository.CreateAsync(__Mapper.Map<LoanExtensionEntity>(request)));

            if (_Response == null)
            {
                _Response = new LoanExtensionResponse
                {
                    Success = false,
                    Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}."
                };
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __LoanExtensionRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<LoanExtensionResponse>> GetAsync(Guid loanUID)
        {
            return __Mapper.Map<IList<LoanExtensionResponse>>(await __LoanExtensionRepository.GetAsync(loanUID));
        }

        public async Task<LoanExtensionResponse> GetByUIDAsync(Guid uid)
        {
            LoanExtensionResponse _Response = __Mapper.Map<LoanExtensionResponse>(await __LoanExtensionRepository.GetByUIDAsync(uid));

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} get {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(UpdateLoanExtensionRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty || !await __LoanExtensionRepository.UpdateAsync(__Mapper.Map<LoanExtensionEntity>(request)))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME}.";
            }

            return _Response;
        }
    }
}
