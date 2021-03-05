using AutoMapper;
using ELMS.WEB.Entities.Equipment;
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
    public class EquipmentBlobManager : IEquipmentBlobManager
    {
        private readonly IMapper __Mapper;
        private readonly IEquipmentBlobRepository __EquipmentBlobRepository;
        private readonly string MODEL_NAME = "Equipment-media association";

        public EquipmentBlobManager(IMapper mapper, IEquipmentBlobRepository equipmentBlobRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentBlobRepository = equipmentBlobRepository ?? throw new ArgumentNullException(nameof(equipmentBlobRepository));
        }

        public async Task<EquipmentBlobResponse> CreateAsync(CreateEquipmentBlobRequest request)
        {
            EquipmentBlobResponse _Response = __Mapper.Map<EquipmentBlobResponse>(await __EquipmentBlobRepository.CreateAsync(__Mapper.Map<EquipmentBlobEntity>(request)));

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EquipmentBlobRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<EquipmentBlobResponse>> GetAsync(Guid equipmentUID)
        {
            return __Mapper.Map<IList<EquipmentBlobResponse>>(await __EquipmentBlobRepository.GetAsync(equipmentUID));
        }

        public async Task<EquipmentBlobResponse> GetByUIDAsync(Guid uid)
        {
            EquipmentBlobResponse _Response = __Mapper.Map<EquipmentBlobResponse>(await __EquipmentBlobRepository.GetByUIDAsync(uid));

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} get {MODEL_NAME}.";
            }

            return _Response;
        }
    }
}
