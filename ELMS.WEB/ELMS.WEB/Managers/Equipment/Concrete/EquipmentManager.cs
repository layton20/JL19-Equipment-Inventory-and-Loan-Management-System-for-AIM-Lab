using ELMS.WEB.Adapters.Equipment;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Concrete
{
    public class EquipmentManager : IEquipmentManager
    {
        private readonly IEquipmentRepository __EquipmentRepository;
        private const string MODEL_NAME = "Equipment";

        public EquipmentManager(IEquipmentRepository equipmentRepository)
        {
            __EquipmentRepository = equipmentRepository ?? throw new ArgumentNullException(nameof(equipmentRepository));
        }

        public async Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request)
        {
            EquipmentResponse _Response = (await __EquipmentRepository.CreateAsync(request.ToEntity())).ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} create ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(DeleteEquipmentViewModel model)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EquipmentRepository.DeleteAsync(model.UID))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} delete ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EquipmentResponse> GetAsync(Guid uid)
        {
            EquipmentResponse _Response = (await __EquipmentRepository.GetAsync(uid)).ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} get ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<EquipmentsResponse> GetAsync()
        {
            EquipmentsResponse _Response = new EquipmentsResponse
            {
                Equipments = (await __EquipmentRepository.GetAsync()).ToList().ToResponse()
            };

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(EquipmentViewModel model)
        {
            BaseResponse _Response = new BaseResponse();

            if (model.UID == Guid.Empty || !await __EquipmentRepository.UpdateAsync(model.ToEntity()))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} update ${MODEL_NAME}.";
            }

            return _Response;
        }
    }
}
