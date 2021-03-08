using ELMS.WEB.Entities.Equipment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Interfaces
{
    public interface IEquipmentBlobRepository
    {
        public Task<EquipmentBlobEntity> CreateAsync(EquipmentBlobEntity entity);

        public Task<IList<EquipmentBlobEntity>> GetAsync(Guid equipmentUID);

        public Task<IList<EquipmentBlobEntity>> GetAsync();

        public Task<EquipmentBlobEntity> GetByUIDAsync(Guid uid);

        public Task<bool> DeleteAsync(Guid uid);
    }
}