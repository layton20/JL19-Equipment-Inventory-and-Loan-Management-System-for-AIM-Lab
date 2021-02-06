using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Email.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Email.Concrete
{
    public class EmailScheduleRepository : IEmailScheduleRepository
    {
        private readonly ApplicationContext __ApplicationContext;

        public EmailScheduleRepository(ApplicationContext applicationContext)
        {
            __ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<EmailScheduleEntity> CreateAsync(EmailScheduleEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __ApplicationContext.EmailSchedules.AddAsync(entity);
            bool _Added = await __ApplicationContext.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            EmailScheduleEntity _EmailSchedule = await __ApplicationContext.EmailSchedules.FindAsync(uid);

            if (_EmailSchedule == null)
            {
                return false;
            }

            __ApplicationContext.EmailSchedules.Remove(_EmailSchedule);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<IList<EmailScheduleEntity>> GetAsync()
        {
            return await __ApplicationContext.EmailSchedules.ToListAsync();
        }

        public async Task<EmailScheduleEntity> GetByUIDAsync(Guid uid)
        {
            return await __ApplicationContext.EmailSchedules.FindAsync(uid);
        }
    }
}
