using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Admin.Concrete
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ApplicationContext __Context;

        public ConfigurationRepository(ApplicationContext context)
        {
            __Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ConfigurationEntity> CreateAsync(ConfigurationEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __Context.Configurations.AddAsync(entity);
            bool _Added = await __Context.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            ConfigurationEntity _ConfigurationEntity = await __Context.Configurations.FirstOrDefaultAsync(x => x.UID == uid);

            if (_ConfigurationEntity == null)
            {
                return false;
            }

            __Context.Configurations.Remove(_ConfigurationEntity);

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IList<ConfigurationEntity>> GetAsync()
        {
            return await __Context.Configurations.ToListAsync();
        }

        public async Task<ConfigurationEntity> GetByNormalizedNameAsync(string name)
        {
            return await __Context.Configurations.Where(x => x.Name.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<ConfigurationEntity> GetByUIDAsync(Guid uid)
        {
            return await __Context.Configurations.FindAsync(uid);
        }

        public async Task<bool> UpdateAsync(ConfigurationEntity entity)
        {
            if (entity.UID == Guid.Empty)
            {
                return false;
            }

            ConfigurationEntity _Entity = await __Context.Configurations.FindAsync(entity.UID);

            if (_Entity == null)
            {
                return false;
            }

            _Entity.Value = entity.Value;
            _Entity.Description = entity.Description;
            _Entity.AmendedTimestamp = entity.AmendedTimestamp;

            return await __Context.SaveChangesAsync() > 0;
        }
    }
}
