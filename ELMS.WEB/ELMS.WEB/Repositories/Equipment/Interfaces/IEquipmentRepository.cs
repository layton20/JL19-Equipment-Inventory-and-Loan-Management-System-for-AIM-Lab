using ELMS.WEB.Entities.Equipment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Interfaces
{
    public interface IEquipmentRepository
    {
        public Task<EquipmentEntity> CreateAsync(EquipmentEntity equipment);
        public Task<EquipmentEntity> GetAsync(Guid uid);
        public Task<IEnumerable<EquipmentEntity>> GetAsync();
        public Task<bool> UpdateAsync(EquipmentEntity equipment);
        public Task<bool> DeleteAsync(Guid uid);
        public Task<bool> BulkCreateAsync(EquipmentEntity equipment, int Quantity);
    }
}
