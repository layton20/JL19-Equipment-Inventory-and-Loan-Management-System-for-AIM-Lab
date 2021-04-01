using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Enums.Equipment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Interfaces
{
    public interface IEquipmentRepository
    {
        public Task<EquipmentEntity> CreateAsync(EquipmentEntity equipment);

        public Task<EquipmentEntity> GetAsync(Guid uid);

        public Task<IList<EquipmentEntity>> GetAsync(IList<Guid> uids);

        public Task<IList<EquipmentEntity>> GetByStatusAsync(Status status);

        public Task<IList<EquipmentEntity>> GetAsync();

        public Task<bool> UpdateAsync(EquipmentEntity equipment);

        public Task<bool> DeleteAsync(Guid uid);

        public Task<bool> UpdateStatusAsync(Guid uid, Status status);

        public Task<IList<EquipmentEntity>> BulkCreateAsync(EquipmentEntity equipment, int Quantity);
    }
}