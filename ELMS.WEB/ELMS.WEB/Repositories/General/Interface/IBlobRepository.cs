using ELMS.WEB.Entities.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.General.Interface
{
    public interface IBlobRepository
    {
        public Task<BlobEntity> CreateAsync(BlobEntity entity);

        public Task<IList<BlobEntity>> GetAsync();

        public Task<BlobEntity> GetAsync(Guid uid);

        public Task<bool> DeleteAsync(Guid uid);

        public Task<bool> UpdateAsync(BlobEntity entity);
    }
}