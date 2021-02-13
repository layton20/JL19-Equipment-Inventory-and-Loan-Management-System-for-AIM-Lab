using ELMS.WEB.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Admin.Interfaces
{
    public interface IBlacklistRepository
    {
        Task<BlacklistEntity> CreateAsync(BlacklistEntity entity);
        Task<bool> UpdateAsync(BlacklistEntity entity);
        Task<bool> DeleteAsync(Guid uid);
        Task<IList<BlacklistEntity>> GetAsync();
        Task<IList<BlacklistEntity>> GetAsync(string email);
        Task<BlacklistEntity> GetByUIDAsync(Guid uid);
    }
}
