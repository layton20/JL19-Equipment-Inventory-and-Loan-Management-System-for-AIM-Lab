using AutoMapper;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Loan;
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
    public class LoanManager : ILoanManager
    {
        private readonly IMapper __Mapper;
        private readonly ILoanRepository __LoanRepository;
        private const string MODEL_NAME = "Loan";

        public LoanManager(IMapper mapper, ILoanRepository loanRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanRepository = loanRepository ?? throw new ArgumentNullException(nameof(loanRepository));
        }

        public async Task<BaseResponse> AcceptTermsAndConditions(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (uid == Guid.Empty || !await __LoanRepository.AcceptTermsAndConditions(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} accept Terms and Conditions for the {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> ChangeStatusAsync(Guid uid, Status status)
        {
            BaseResponse _Response = new BaseResponse();

            if (uid == Guid.Empty || !await __LoanRepository.ChangeStatusAsync(uid, status))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update status for the {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<LoanResponse> CreateAsync(CreateLoanRequest request)
        {
            LoanResponse _Response = __Mapper.Map<LoanResponse>(await __LoanRepository.CreateAsync(__Mapper.Map<LoanEntity>(request), request.Equipment));

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} create ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<LoanResponse>> GetAsync(Guid equipmentUID, bool all = false)
        {
            return __Mapper.Map<IList<LoanResponse>>(await __LoanRepository.GetAsync(equipmentUID, all));
        }

        public async Task<LoanResponse> GetByUIDAsync(Guid uid)
        {
            return __Mapper.Map<LoanResponse>(await __LoanRepository.GetByUIDAsync(uid));
        }

        public async Task<BaseResponse> UpdateAsync(UpdateLoanRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty || !await __LoanRepository.UpdateAsync(__Mapper.Map<LoanEntity>(request)))
            {
                _Response.Success = false;
                _Response.Message = $"Error: {GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<LoanResponse>> GetAsync()
        {
            return __Mapper.Map<IList<LoanResponse>>((await __LoanRepository.GetAsync()));
        }

        public async Task<IntResponse> GetCountByStatus(Status status)
        {
            IntResponse _Response = new IntResponse
            {
                Value = await __LoanRepository.GetCountByStatus(status)
            };

            return _Response;
        }

        public async Task<IList<LoanResponse>> GetByUserAsync(string loaneeEmail)
        {
            return __Mapper.Map<IList<LoanResponse>>(await __LoanRepository.GetByUserAsync(loaneeEmail));
        }
    }
}