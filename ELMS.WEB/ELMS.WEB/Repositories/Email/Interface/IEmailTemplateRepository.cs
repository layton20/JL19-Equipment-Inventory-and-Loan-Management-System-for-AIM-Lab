using ELMS.WEB.Entities.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Email.Interface
{
    public interface IEmailTemplateRepository
    {
        public Task<EmailTemplateEntity> CreateAsync(EmailTemplateEntity entity);

        public Task<IList<EmailTemplateEntity>> GetAsync();

        public Task<EmailTemplateEntity> GetByUIDAsync(Guid uid);

        public Task<bool> UpdateAsync(EmailTemplateEntity entity);

        public Task<bool> DeleteAsync(Guid uid);
    }
}