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
    public class LoanEquipmentRepository : ILoanEquipmentRepository
    {
        private readonly ApplicationContext __ApplicationContext;

        public LoanEquipmentRepository(ApplicationContext applicationContext)
        {
            __ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            LoanEquipmentEntity _LoanEquipment = await __ApplicationContext.LoanEquipmentList.FindAsync(uid);

            if (_LoanEquipment == null)
            {
                return false;
            }

            __ApplicationContext.LoanEquipmentList.Remove(_LoanEquipment);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<IList<LoanEquipmentEntity>> GetAsync(Guid loanUID)
        {
            return await __ApplicationContext.LoanEquipmentList.Where(x => x.LoanUID == loanUID).Include(x => x.Equipment).ToListAsync();
        }

        public async Task<IList<LoanEquipmentEntity>> GetAsync()
        {
            return await __ApplicationContext.LoanEquipmentList.ToListAsync();
        }

        public async Task<IList<LoanEquipmentEntity>> GetByEquipmentAsync(Guid equipmentUID)
        {
            return await __ApplicationContext.LoanEquipmentList.Where(x => x.EquipmentUID == equipmentUID).Include(x => x.Equipment).ToListAsync();
        }
    }
}