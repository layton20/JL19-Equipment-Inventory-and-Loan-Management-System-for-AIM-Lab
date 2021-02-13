using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Interfaces
{
    public interface IEquipmentManager
    {
        public Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request);

        public Task<EquipmentResponse> GetAsync(Guid uid);

        public Task<EquipmentListResponse> GetAsync();

        public Task<EquipmentListResponse> GetAsync(IList<Guid> uids);

        public Task<BaseResponse> UpdateAsync(EquipmentViewModel model);

        public Task<BaseResponse> DeleteAsync(DeleteEquipmentViewModel model);

        public Task<IList<EquipmentResponse>> BulkCreateAsync(CreateEquipmentRequest request);
    }
}