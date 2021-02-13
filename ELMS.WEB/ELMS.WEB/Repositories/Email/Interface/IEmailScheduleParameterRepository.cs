using ELMS.WEB.Entities.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Email.Interface
{
    public interface IEmailScheduleParameterRepository
    {
        public Task<EmailScheduleParameterEntity> CreateAsync(EmailScheduleParameterEntity entity);

        public Task<IList<EmailScheduleParameterEntity>> GetAsync(Guid scheduleUID);

        public Task<bool> DeleteAsync(Guid scheduleUID);
    }
}