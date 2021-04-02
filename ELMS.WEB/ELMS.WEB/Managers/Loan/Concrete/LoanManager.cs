using AutoMapper;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Loan;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using ELMS.WEB.Repositories.Loan.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Loan.Concrete
{
    public class LoanManager : ILoanManager
    {
        private readonly IMapper __Mapper;
        private readonly ILoanRepository __LoanRepository;
        private readonly IEquipmentRepository __EquipmentRepository;
        private readonly ILoanEquipmentRepository __LoanEquipmentRepository;
        private readonly ILoanExtensionRepository __LoanExtensionRepository;
        private const string MODEL_NAME = "Loan";

        public LoanManager(IMapper mapper, ILoanRepository loanRepository, IEquipmentRepository equipmentRepository, ILoanEquipmentRepository loanEquipmentRepository, ILoanExtensionRepository loanExtensionRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanRepository = loanRepository ?? throw new ArgumentNullException(nameof(loanRepository));
            __EquipmentRepository = equipmentRepository ?? throw new ArgumentNullException(nameof(equipmentRepository));
            __LoanEquipmentRepository = loanEquipmentRepository ?? throw new ArgumentNullException(nameof(loanEquipmentRepository));
            __LoanExtensionRepository = loanExtensionRepository ?? throw new ArgumentNullException(nameof(loanExtensionRepository));
        }

        public async Task<BaseResponse> AcceptTermsAndConditions(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (uid == Guid.Empty || !await __LoanRepository.AcceptTermsAndConditionsAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} accept Terms and Conditions for the {MODEL_NAME}.";
            }

            IList<LoanEquipmentEntity> _LoanEquipmentResponses = await __LoanEquipmentRepository.GetAsync(uid);

            foreach (LoanEquipmentEntity loanEquipment in _LoanEquipmentResponses)
            {
                await __EquipmentRepository.UpdateStatusAsync(loanEquipment.EquipmentUID, Enums.Equipment.Status.ActiveLoan);
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
            IList<LoanEntity> _LoanEntities = await __LoanRepository.GetAsync(all);

            foreach (LoanEntity loan in _LoanEntities)
            {
                loan.ExpiryTimestamp = await GetExpiryDate(loan.UID);
            }

            return __Mapper.Map<IList<LoanResponse>>(_LoanEntities);
        }

        public async Task<LoanResponse> GetByUIDAsync(Guid uid)
        {
            LoanResponse _Response = __Mapper.Map<LoanResponse>(await __LoanRepository.GetByUIDAsync(uid));

            IList<Guid> _EquipmentUIDs = (await __LoanEquipmentRepository.GetAsync(_Response.UID)).Select(x => x.EquipmentUID).ToList();
            _Response.EquipmentList = __Mapper.Map<IList<EquipmentResponse>>(await __EquipmentRepository.GetAsync(_EquipmentUIDs));
            _Response.ExpiryTimestamp = await GetExpiryDate(uid);

            return _Response;
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
            IList<LoanResponse> _LoanResponses = __Mapper.Map<IList<LoanResponse>>(await __LoanRepository.GetAsync());

            foreach (LoanResponse response in _LoanResponses)
            {
                IList<Guid> _EquipmentUIDs = (await __LoanEquipmentRepository.GetAsync(response.UID)).Select(x => x.EquipmentUID).ToList();
                response.EquipmentList = __Mapper.Map<IList<EquipmentResponse>>(await __EquipmentRepository.GetAsync(_EquipmentUIDs));
                response.ExpiryTimestamp = await GetExpiryDate(response.UID);
                response.Extensions = __Mapper.Map<IList<LoanExtensionResponse>>(await __LoanExtensionRepository.GetAsync(response.UID));
            }

            return _LoanResponses;
        }

        public async Task<IntResponse> GetCountByStatus(Status status)
        {
            IntResponse _Response = new IntResponse
            {
                Value = await __LoanRepository.GetCountByStatus(status)
            };

            return _Response;
        }

        public async Task<IList<LoanResponse>> GetByLoaneeAsync(string loaneeEmail)
        {
            IList<LoanResponse> _LoanResponses = __Mapper.Map<IList<LoanResponse>>(await __LoanRepository.GetByLoaneeAsync(loaneeEmail));

            foreach (LoanResponse loanResponse in _LoanResponses)
            {
                IList<LoanEquipmentEntity> _LoanEquipments = await __LoanEquipmentRepository.GetAsync(loanResponse.UID);
                loanResponse.EquipmentList = __Mapper.Map<IList<EquipmentResponse>>(_LoanEquipments.Select(x => x.Equipment));
                loanResponse.ExpiryTimestamp = await GetExpiryDate(loanResponse.UID);
                loanResponse.Extensions = __Mapper.Map<IList<LoanExtensionResponse>>(await __LoanExtensionRepository.GetAsync(loanResponse.UID));
            }

            return _LoanResponses;
        }

        public async Task<IList<LoanResponse>> GetByLoanerAsync(string loanerEmail)
        {
            IList<LoanResponse> _LoanResponses = __Mapper.Map<IList<LoanResponse>>(await __LoanRepository.GetByLoaneeAsync(loanerEmail));

            foreach (LoanResponse loanResponse in _LoanResponses)
            {
                IList<LoanEquipmentEntity> _LoanEquipments = await __LoanEquipmentRepository.GetAsync(loanResponse.UID);
                loanResponse.EquipmentList = __Mapper.Map<IList<EquipmentResponse>>(_LoanEquipments.Select(x => x.Equipment));
                loanResponse.ExpiryTimestamp = await GetExpiryDate(loanResponse.UID);
                loanResponse.Extensions = __Mapper.Map<IList<LoanExtensionResponse>>(await __LoanExtensionRepository.GetAsync(loanResponse.UID));
            }

            return _LoanResponses;
        }

        public async Task<DateTime> GetExpiryDate(Guid loanUID)
        {
            LoanEntity _Response = await __LoanRepository.GetByUIDAsync(loanUID);

            if (_Response == null)
            {
                return DateTime.MinValue;
            }

            IList<LoanExtensionEntity> _Extensions = (await __LoanExtensionRepository.GetAsync(loanUID))?.OrderByDescending(x => x.NewExpiryDate)?.ToList();

            if (_Extensions == null || _Extensions.Count <= 0)
            {
                return _Response.ExpiryTimestamp;
            }

            return _Extensions[0].NewExpiryDate;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __LoanRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> CompleteLoanAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __LoanRepository.CompleteLoanAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} complete {MODEL_NAME}.";
            }

            return _Response;
        }
    }
}