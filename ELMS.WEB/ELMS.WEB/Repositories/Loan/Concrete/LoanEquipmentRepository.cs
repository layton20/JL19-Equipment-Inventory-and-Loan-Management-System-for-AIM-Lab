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

        public async Task<IList<LoanEquipmentEntity>> GetAsync(Guid loanUID)
        {
            return await __ApplicationContext.LoanEquipmentList.Where(x => x.LoanUID == loanUID).ToListAsync();
        }
    }
}