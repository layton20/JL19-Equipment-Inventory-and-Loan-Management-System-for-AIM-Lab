using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Interfaces
{
    public interface IEquipmentManager
    {
        public Task<EquipmentResponse> CreateAsync(CreateEquipmentViewModel model);
        public Task<EquipmentResponse> GetAsync(Guid uid);
        public Task<EquipmentsResponse> GetAsync();
        public Task<BaseResponse> UpdateAsync(UpdateEquipmentViewModel model);
        public Task<BaseResponse> DeleteAsync(DeleteEquipmentViewModel model);
    }
}
