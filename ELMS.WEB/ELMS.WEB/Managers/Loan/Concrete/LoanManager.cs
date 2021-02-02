using ELMS.WEB.Adapters.Loan;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Loan;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Identity.Interface;
using ELMS.WEB.Repositories.Loan.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Concrete
{
    public class LoanManager : ILoanManager
    {
        private readonly ILoanRepository __LoanRepository;
        private readonly IUserRepository __UserRepository;
        private const string MODEL_NAME = "Loan";

        public LoanManager(ILoanRepository loanRepository, IUserRepository userRepository)
        {
            __LoanRepository = loanRepository ?? throw new ArgumentNullException(nameof(loanRepository));
            __UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<BaseResponse> AcceptTermsAndConditions(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (uid == Guid.Empty || !await __LoanRepository.AcceptTermsAndConditions(uid))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} accept Terms and Conditions for the ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> ChangeStatusAsync(Guid uid, Status status)
        {
            BaseResponse _Response = new BaseResponse();

            if (uid == Guid.Empty || !await __LoanRepository.ChangeStatusAsync(uid, status))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} update status for the ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<LoanResponse> CreateAsync(CreateLoanRequest request)
        {
            LoanResponse _Response = (await __LoanRepository.CreateAsync(request.ToEntity(), request.EquipmentList)).ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} create ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<LoanResponse>> GetAsync(Guid equipmentUID, bool all = false)
        {
            return (await __LoanRepository.GetAsync(equipmentUID, all)).ToResponse();
        }

        public async Task<LoanResponse> GetByUIDAsync(Guid uid)
        {
            return (await __LoanRepository.GetByUIDAsync(uid)).ToResponse();
        }

        public async Task<BaseResponse> UpdateAsync(UpdateLoanRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty || !await __LoanRepository.UpdateAsync(request.ToEntity()))
            {
                _Response.Success = false;
                _Response.Message = $"Error: {GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<LoanResponse>> GetAsync()
        {
            return (await __LoanRepository.GetAsync()).ToResponse();
        }

        public async Task<IntResponse> GetCountByStatus(Status status)
        {
            IntResponse _Response = new IntResponse
            {
                Value = await __LoanRepository.GetCountByStatus(status)
            };

            return _Response;
        }
    }
}
