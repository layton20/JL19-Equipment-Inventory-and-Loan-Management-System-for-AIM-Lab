using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Interfaces
{
    public interface IEquipmentBlobManager
    {
        public Task<EquipmentBlobResponse> CreateAsync(CreateEquipmentBlobRequest request);
        public Task<IList<EquipmentBlobResponse>> GetAsync(Guid equipmentUID);
        public Task<IList<EquipmentBlobResponse>> GetAsync();
        public Task<EquipmentBlobResponse> GetByUIDAsync(Guid uid);
        public Task<BaseResponse> DeleteAsync(Guid uid);
    }
}
