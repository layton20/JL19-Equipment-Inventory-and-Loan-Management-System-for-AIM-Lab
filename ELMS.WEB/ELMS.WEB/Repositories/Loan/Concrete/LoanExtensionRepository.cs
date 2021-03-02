using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Loan.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Loan.Concrete
{
    public class LoanExtensionRepository : ILoanExtensionRepository
    {
        private readonly ApplicationContext __ApplicationContext;

        public LoanExtensionRepository(ApplicationContext applicationContext)
        {
            __ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<LoanExtensionEntity> CreateAsync(LoanExtensionEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return null;
            }

            entity.NewExpiryDate = entity.NewExpiryDate.Date.AddDays(1).AddSeconds(-1);
            await __ApplicationContext.LoanExtensions.AddAsync(entity);
            return (await __ApplicationContext.SaveChangesAsync() > 0) ? entity : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            LoanExtensionEntity _Entity = await __ApplicationContext.LoanExtensions.FindAsync(uid);

            if (_Entity == null)
            {
                return false;
            }

            __ApplicationContext.LoanExtensions.Remove(_Entity);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<IList<LoanExtensionEntity>> GetAsync(Guid loanUid)
        {
            return await __ApplicationContext.LoanExtensions.Where(x => x.LoanUID == loanUid).ToListAsync();
        }

        public async Task<LoanExtensionEntity> GetByUIDAsync(Guid uid)
        {
            return await __ApplicationContext.LoanExtensions.FindAsync(uid);
        }

        public async Task<bool> UpdateAsync(LoanExtensionEntity entity)
        {
            if (entity == null || entity.UID == Guid.Empty)
            {
                return false;
            }

            LoanExtensionEntity _Entity = await __ApplicationContext.LoanExtensions.FindAsync(entity.UID);

            if (_Entity == null)
            {
                return false;
            }

            _Entity.ExtenderEmail = entity.ExtenderEmail;
            _Entity.PreviousExpiryDate = entity.PreviousExpiryDate.Date;
            _Entity.NewExpiryDate = entity.NewExpiryDate.Date.AddDays(1).AddSeconds(-1);
            _Entity.AmendedTimestamp = DateTime.Now;

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }
    }
}