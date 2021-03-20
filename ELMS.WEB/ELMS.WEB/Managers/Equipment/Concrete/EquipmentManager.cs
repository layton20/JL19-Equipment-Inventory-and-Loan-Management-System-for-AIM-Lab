using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Concrete
{
    public class EquipmentManager : IEquipmentManager
    {
        private readonly IMapper __Mapper;
        private readonly IEquipmentRepository __EquipmentRepository;
        private const string MODEL_NAME = "Equipment";

        public EquipmentManager(IMapper mapper, IEquipmentRepository equipmentRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentRepository = equipmentRepository ?? throw new ArgumentNullException(nameof(equipmentRepository));
        }

        public async Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request)
        {
            EquipmentResponse _Response = __Mapper.Map<EquipmentResponse>(await __EquipmentRepository.CreateAsync(__Mapper.Map<EquipmentEntity>(request)));

            if (_Response == null)
            {
                _Response = new EquipmentResponse();
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<EquipmentResponse>> BulkCreateAsync(CreateEquipmentRequest request)
        {
            IList<EquipmentEntity> _Entities = await __EquipmentRepository.BulkCreateAsync(__Mapper.Map<EquipmentEntity>(request), request.Quantity);

            return _Entities == null ? null : __Mapper.Map<IList<EquipmentResponse>>(_Entities);
        }

        public async Task<BaseResponse> DeleteAsync(DeleteEquipmentViewModel model)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EquipmentRepository.DeleteAsync(model.UID))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EquipmentResponse> GetAsync(Guid uid)
        {
            EquipmentResponse _Response = __Mapper.Map<EquipmentResponse>(await __EquipmentRepository.GetAsync(uid));

            if (_Response == null)
            {
                return new EquipmentResponse
                {
                    Success = false,
                    Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} get {MODEL_NAME}."
                };
            }

            return _Response;
        }

        public async Task<IList<EquipmentResponse>> GetAsync()
        {
            return __Mapper.Map<IList<EquipmentResponse>>(await __EquipmentRepository.GetAsync());
        }

        public async Task<EquipmentListResponse> GetAsync(IList<Guid> uids)
        {
            EquipmentListResponse _Response = new EquipmentListResponse
            {
                Equipments = __Mapper.Map<IList<EquipmentResponse>>(await __EquipmentRepository.GetAsync(uids))
            };

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(EquipmentViewModel model)
        {
            BaseResponse _Response = new BaseResponse();

            if (model.UID == Guid.Empty || !await __EquipmentRepository.UpdateAsync(__Mapper.Map<EquipmentEntity>(model)))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME} details.";
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {MODEL_NAME} details.";
            }

            return _Response;
        }

        public async Task<BaseResponse> UpdateStatusAsync(Guid uid, Status status)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EquipmentRepository.UpdateStatusAsync(uid, status))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update status of {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<EquipmentResponse>> GetByStatusAsync(Status status)
        {
            return __Mapper.Map<IList<EquipmentResponse>>(await __EquipmentRepository.GetByStatusAsync(status));
        }
    }
}