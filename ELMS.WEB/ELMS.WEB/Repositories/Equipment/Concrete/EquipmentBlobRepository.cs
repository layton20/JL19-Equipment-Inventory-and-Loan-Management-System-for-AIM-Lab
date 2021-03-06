using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Concrete
{
    public class EquipmentBlobRepository : IEquipmentBlobRepository
    {
        private readonly ApplicationContext __ApplicationContext;

        public EquipmentBlobRepository(ApplicationContext applicationContext)
        {
            __ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<EquipmentBlobEntity> CreateAsync(EquipmentBlobEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __ApplicationContext.EquipmentBlobs.AddAsync(entity);
            bool _Added = await __ApplicationContext.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            EquipmentBlobEntity _Entity = await __ApplicationContext.EquipmentBlobs.FindAsync(uid);

            if (_Entity == null)
            {
                return false;
            }

            __ApplicationContext.EquipmentBlobs.Remove(_Entity);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<IList<EquipmentBlobEntity>> GetAsync(Guid equipmentUID)
        {
            return await __ApplicationContext.EquipmentBlobs.Include(x => x.Equipment).Include(x => x.Blob).Where(x => x.EquipmentUID == equipmentUID).ToListAsync();
        }

        public async Task<IList<EquipmentBlobEntity>> GetAsync()
        {
            return await __ApplicationContext.EquipmentBlobs.Include(x => x.Equipment).Include(x => x.Blob).ToListAsync();
        }

        public async Task<EquipmentBlobEntity> GetByUIDAsync(Guid uid)
        {
            return await __ApplicationContext.EquipmentBlobs.Where(x => x.UID == uid).Include(x => x.Blob).FirstOrDefaultAsync();
        }
    }
}
