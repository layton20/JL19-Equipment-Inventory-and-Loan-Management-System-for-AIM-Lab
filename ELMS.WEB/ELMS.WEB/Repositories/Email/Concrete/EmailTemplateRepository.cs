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
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly ApplicationContext __Context;

        public EmailTemplateRepository(ApplicationContext applicationContext)
        {
            __Context = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<EmailTemplateEntity> CreateAsync(EmailTemplateEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            await __Context.EmailTemplates.AddAsync(entity);
            bool _Added = await __Context.SaveChangesAsync() > 0;

            return _Added ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            EmailTemplateEntity _EmailTempalte = await __Context.EmailTemplates.FirstOrDefaultAsync(x => x.UID == uid);

            if (_EmailTempalte == null)
            {
                return false;
            }

            __Context.EmailTemplates.Remove(_EmailTempalte);

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IList<EmailTemplateEntity>> GetAsync()
        {
            return await __Context.EmailTemplates.ToListAsync();
        }

        public async Task<EmailTemplateEntity> GetByUIDAsync(Guid uid)
        {
            return await __Context.EmailTemplates.FindAsync(uid);
        }

        public async Task<bool> UpdateAsync(EmailTemplateEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return false;
            }

            EmailTemplateEntity _EmailTemplate = await __Context.EmailTemplates.FindAsync(entity.UID);

            if (_EmailTemplate == null)
            {
                return false;
            }

            _EmailTemplate.Name = entity.Name;
            _EmailTemplate.Subject = entity.Subject;
            _EmailTemplate.Body = entity.Body;
            _EmailTemplate.TemplateType = entity.TemplateType;
            _EmailTemplate.AmendedTimestamp = DateTime.Now;

            return await __Context.SaveChangesAsync() > 0;
        }
    }
}
