using ELMS.WEB.Entities.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Email.Interface
{
    public interface IEmailScheduleRepository
    {
        public Task<EmailScheduleEntity> CreateAsync(EmailScheduleEntity entity);

        public Task<IList<EmailScheduleEntity>> GetAsync();

        public Task<IList<EmailScheduleEntity>> GetEmailsToSendAsync();

        public Task<EmailScheduleEntity> GetByUIDAsync(Guid uid);

        public Task<bool> UpdateSentAsync(Guid uid, bool sent);

        public Task<bool> DeleteAsync(Guid uid);
    }
}