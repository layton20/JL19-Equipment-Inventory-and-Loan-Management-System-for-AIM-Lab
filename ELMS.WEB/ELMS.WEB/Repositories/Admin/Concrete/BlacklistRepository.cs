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
    public class BlacklistRepository : IBlacklistRepository
    {
        private readonly ApplicationContext __Context;

        public BlacklistRepository(ApplicationContext context)
        {
            __Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BlacklistEntity> CreateAsync(BlacklistEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __Context.Blacklists.AddAsync(entity);
            bool _Added = await __Context.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            BlacklistEntity _BlacklistEntity = await __Context.Blacklists.FirstOrDefaultAsync(x => x.UID == uid);

            if (_BlacklistEntity == null)
            {
                return false;
            }

            __Context.Blacklists.Remove(_BlacklistEntity);

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IList<BlacklistEntity>> GetAsync()
        {
            return await __Context.Blacklists.ToListAsync();
        }

        public async Task<IList<BlacklistEntity>> GetAsync(string email)
        {
            return await __Context.Blacklists.Where(b => b.Email.ToUpper() == email.ToUpper()).ToListAsync();
        }

        public async Task<BlacklistEntity> GetByUIDAsync(Guid uid)
        {
            return await __Context.Blacklists.FirstOrDefaultAsync(x => x.UID == uid);
        }

        public async Task<bool> UpdateAsync(BlacklistEntity entity)
        {
            if (entity.UID == Guid.Empty)
            {
                return false;
            }

            BlacklistEntity _Entity = await __Context.Blacklists.FindAsync(entity.UID);

            if (_Entity == null)
            {
                return false;
            }

            _Entity.Reason = entity.Reason;
            _Entity.Type = entity.Type;
            _Entity.Active = entity.Active;
            _Entity.AmendedTimestamp = entity.AmendedTimestamp;

            return await __Context.SaveChangesAsync() > 0;
        }
    }
}
