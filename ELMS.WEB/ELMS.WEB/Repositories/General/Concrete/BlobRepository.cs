using ELMS.WEB.Entities.General;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.General.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.General.Concrete
{
    public class BlobRepository : IBlobRepository
    {
        private readonly ApplicationContext __Context;

        public BlobRepository(ApplicationContext context)
        {
            __Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BlobEntity> CreateAsync(BlobEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __Context.Blobs.AddAsync(entity);
            bool _Added = await __Context.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            BlobEntity _Blob = await __Context.Blobs.FirstOrDefaultAsync(x => x.UID == uid);

            if (_Blob == null)
            {
                return false;
            }

            __Context.Blobs.Remove(_Blob);

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IList<BlobEntity>> GetAsync()
        {
            return await __Context.Blobs.ToListAsync();
        }

        public async Task<BlobEntity> GetAsync(Guid uid)
        {
            return await __Context.Blobs.FindAsync(uid);
        }

        public async Task<bool> UpdateAsync(BlobEntity entity)
        {
            if (entity.UID == Guid.Empty)
            {
                return false;
            }

            BlobEntity _Entity = await __Context.Blobs.FindAsync(entity.UID);

            if (_Entity == null)
            {
                return false;
            }

            _Entity.Name = entity.Name;
            _Entity.Path = entity.Path;
            _Entity.AmendedTimestamp = DateTime.Now;

            return await __Context.SaveChangesAsync() > 0;
        }
    }
}