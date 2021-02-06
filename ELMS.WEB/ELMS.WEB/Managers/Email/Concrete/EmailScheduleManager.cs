using ELMS.WEB.Adapters.Email;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Enums.Email;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Repositories.Email.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Concrete
{
    public class EmailScheduleManager : IEmailScheduleManager
    {
        private readonly IEmailScheduleRepository __EmailScheduleRepository;
        private readonly IEmailScheduleParameterRepository __EmailScheduleParameterRepository;
        private const string MODEL_NAME = "Email Schedule";

        public EmailScheduleManager(IEmailScheduleRepository emailScheduleRepository, IEmailScheduleParameterRepository emailScheduleParameterRepository)
        {
            __EmailScheduleRepository = emailScheduleRepository ?? throw new ArgumentNullException(nameof(emailScheduleRepository));
            __EmailScheduleParameterRepository = emailScheduleParameterRepository ?? throw new ArgumentNullException(nameof(emailScheduleParameterRepository));
        }

        public async Task<EmailScheduleResponse> CreateAsync(CreateEmailScheduleRequest request)
        {
            EmailScheduleResponse _Response = (await __EmailScheduleRepository.CreateAsync(request?.ToEntity()))?.ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<EmailScheduleResponse>> GetAsync()
        {
            return (await __EmailScheduleRepository.GetAsync()).ToResponse();
        }

        public async Task<EmailScheduleResponse> GetByUIDAsync(Guid uid)
        {
            return (await __EmailScheduleRepository.GetByUIDAsync(uid)).ToResponse();
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailScheduleRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> CreateEquipmentWarrantyAsync(EquipmentResponse equipment, string baseURL)
        {
            // Nearly Expired Schedule
            CreateEmailScheduleRequest _NearlyExpiredScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Nearly_Expired_Warranty,
                RecipientEmailAddress = "lej1@aston.ac.uk",
                SendTimestamp = equipment.WarrantyExpirationDate.Date.AddDays(-3)
            };
            EmailScheduleEntity _NearlyExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_NearlyExpiredScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterNearlyExpiredRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _NearlyExpiredScheduleEntity.UID,
                Name = "Warranty_Expiry_URL",
                Value = $"{baseURL}/Equipment/EquipmentDetails?uid={equipment.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterNearlyExpiredRequest.ToEntity());

            // Expired Schedule
            CreateEmailScheduleRequest _ExpiredScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Expired_Warranty,
                RecipientEmailAddress = "lej1@aston.ac.uk",
                SendTimestamp = equipment.WarrantyExpirationDate.Date
            };
            EmailScheduleEntity _ExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_ExpiredScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterExpiredRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _ExpiredScheduleEntity.UID,
                Name = "Warranty_Expiry_URL",
                Value = $"{baseURL}/Equipment/EquipmentDetails?uid={equipment.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterExpiredRequest.ToEntity());

            return new BaseResponse();
        }

        public async Task<BaseResponse> BulkCreateEquipmentWarrantyAsync(IList<EquipmentResponse> equipmentList, string baseURL)
        {
            foreach (EquipmentResponse equipment in equipmentList)
            {
                // Nearly Expired Schedule
                CreateEmailScheduleRequest _NearlyExpiredScheduleRequest = new CreateEmailScheduleRequest
                {
                    EmailType = EmailType.Nearly_Expired_Warranty,
                    RecipientEmailAddress = "lej1@aston.ac.uk",
                    SendTimestamp = equipment.WarrantyExpirationDate.Date.AddDays(-3)
                };
                EmailScheduleEntity _NearlyExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_NearlyExpiredScheduleRequest.ToEntity());

                CreateEmailScheduleParameterRequest _ParameterNearlyExpiredRequest = new CreateEmailScheduleParameterRequest
                {
                    EmailScheduleUID = _NearlyExpiredScheduleEntity.UID,
                    Name = "Warranty_Expiry_URL",
                    Value = $"{baseURL}/Equipment/EquipmentDetails?uid={equipment.UID}"
                };
                await __EmailScheduleParameterRepository.CreateAsync(_ParameterNearlyExpiredRequest.ToEntity());

                // Expired Schedule
                CreateEmailScheduleRequest _ExpiredScheduleRequest = new CreateEmailScheduleRequest
                {
                    EmailType = EmailType.Expired_Warranty,
                    RecipientEmailAddress = "lej1@aston.ac.uk",
                    SendTimestamp = equipment.WarrantyExpirationDate.Date
                };
                EmailScheduleEntity _ExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_ExpiredScheduleRequest.ToEntity());

                CreateEmailScheduleParameterRequest _ParameterExpiredRequest = new CreateEmailScheduleParameterRequest
                {
                    EmailScheduleUID = _ExpiredScheduleEntity.UID,
                    Name = "Warranty_Expiry_URL",
                    Value = $"{baseURL}/Equipment/EquipmentDetails?uid={equipment.UID}"
                };
                await __EmailScheduleParameterRepository.CreateAsync(_ParameterExpiredRequest.ToEntity());
            }

            return new BaseResponse();
        }
    }
}