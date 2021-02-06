using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Email.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Email.Concrete
{
    public class EmailScheduleParameterRepository : IEmailScheduleParameterRepository
    {
        private readonly ApplicationContext __ApplicationContext;

        public EmailScheduleParameterRepository(ApplicationContext applicationContext)
        {
            __ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<EmailScheduleParameterEntity> CreateAsync(EmailScheduleParameterEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __ApplicationContext.EmailScheduleParameters.AddAsync(entity);
            bool _Added = await __ApplicationContext.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid scheduleUID)
        {
            if (scheduleUID == Guid.Empty)
            {
                return false;
            }

            IList<EmailScheduleParameterEntity> _Entities = await __ApplicationContext.EmailScheduleParameters.Where(x => x.EmailScheduleUID == scheduleUID)?.ToListAsync();

            if (_Entities.Count <= 0)
            {
                return false;
            }

            __ApplicationContext.EmailScheduleParameters.RemoveRange(_Entities);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<IList<EmailScheduleParameterEntity>> GetAsync(Guid scheduleUID)
        {
            if (scheduleUID == Guid.Empty)
            {
                return null;
            }

            return await __ApplicationContext.EmailScheduleParameters.Where(x => x.EmailScheduleUID == scheduleUID)?.ToListAsync();
        }
    }
}
