using ELMS.WEB.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Admin.Interfaces
{
    public interface IConfigurationRepository
    {
        public Task<ConfigurationEntity> CreateAsync(ConfigurationEntity entity);
        public Task<bool> DeleteAsync(Guid uid);
        public Task<IList<ConfigurationEntity>> GetAsync();
        public Task<ConfigurationEntity> GetByUIDAsync(Guid uid);
        public Task<ConfigurationEntity> GetByNormalizedNameAsync(string name);
        public Task<bool> UpdateAsync(ConfigurationEntity entity);
    }
}
