using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Loan;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Loan.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Loan.Concrete
{
    public class LoanRepository : ILoanRepository
    {
        private readonly ApplicationContext __ApplicationContext;

        public LoanRepository(ApplicationContext applicationContext)
        {
            __ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<bool> AcceptTermsAndConditions(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            LoanEntity _Loan = await __ApplicationContext.Loans.FirstOrDefaultAsync(x => x.UID == uid);

            if (_Loan == null)
            {
                return false;
            }

            if (_Loan.FromTimestamp <= DateTime.Now)
            {
                _Loan.Status = Status.ActiveLoan;
            }
            else
            {
                _Loan.Status = Status.InactiveLoan;
            }

            _Loan.AcceptedTermsAndConditions = true;

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeStatusAsync(Guid uid, Status status)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            LoanEntity _Loan = await __ApplicationContext.Loans.FirstOrDefaultAsync(x => x.UID == uid);

            if (_Loan == null)
            {
                return false;
            }

            _Loan.Status = status;

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CompleteLoanAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            LoanEntity _Entity = await __ApplicationContext.Loans.FindAsync(uid);

            if (_Entity == null || _Entity?.CompletedTimestamp == null)
            {
                return false;
            }

            _Entity.CompletedTimestamp = DateTime.Now;
            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<LoanEntity> CreateAsync(LoanEntity loan, IList<Guid> equipmentList)
        {
            if (loan == null || equipmentList.Count <= 0 || loan.UID == Guid.Empty)
            {
                return null;
            }

            IList<EquipmentEntity> _EquipmentList = await __ApplicationContext.Equipment.Where(x => equipmentList.Contains(x.UID) && x.Status == Enums.Equipment.Status.Available).ToListAsync();

            if (_EquipmentList.Count <= 0)
            {
                return null;
            }

            loan.ExpiryTimestamp = loan.ExpiryTimestamp.Date.AddDays(1).AddSeconds(-1);
            await __ApplicationContext.Loans.AddAsync(loan);
            await __ApplicationContext.LoanEquipmentList.AddRangeAsync(_EquipmentList.Select(x => new LoanEquipmentEntity
            {
                EquipmentUID = x.UID,
                LoanUID = loan.UID
            }).ToList());

            bool _Added = await __ApplicationContext.SaveChangesAsync() > 0;
            return loan;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            LoanEntity _Loan = await __ApplicationContext.Loans.FindAsync(uid);

            if (_Loan == null)
            {
                return false;
            }

            __ApplicationContext.Loans.Remove(_Loan);

            IList<LoanEquipmentEntity> _LoanEquipmentList = await __ApplicationContext.LoanEquipmentList.Where(x => x.LoanUID == uid).ToListAsync();

            __ApplicationContext.LoanEquipmentList.RemoveRange(_LoanEquipmentList);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }

        public async Task<IList<LoanEntity>> GetAsync(Guid equipmentUID, bool all = false)
        {
            if (all)
            {
                return await __ApplicationContext.Loans.ToListAsync();
            }

            return await __ApplicationContext.Loans.Where(x => x.Status == Status.ActiveLoan).ToListAsync();
        }

        public async Task<IList<LoanEntity>> GetAsync()
        {
            return await __ApplicationContext.Loans.ToListAsync();
        }

        public async Task<LoanEntity> GetByUIDAsync(Guid uid)
        {
            return await __ApplicationContext.Loans.FirstOrDefaultAsync(x => x.UID == uid);
        }

        public async Task<IList<LoanEntity>> GetByUserAsync(string email)
        {
            return await __ApplicationContext.Loans.Where(x => x.LoaneeEmail.ToUpper() == email.ToUpper()).ToListAsync();
        }

        public async Task<int> GetCountByStatus(Status status)
        {
            return await __ApplicationContext.Loans.Where(x => x.Status == status).CountAsync();
        }

        public async Task<bool> UpdateAsync(LoanEntity loan)
        {
            if (loan.UID == Guid.Empty)
            {
                return false;
            }

            LoanEntity _Loan = await __ApplicationContext.Loans.FindAsync(loan.UID);

            if (_Loan == null)
            {
                return false;
            }

            _Loan.AcceptedTermsAndConditions = loan.AcceptedTermsAndConditions;
            _Loan.Status = loan.Status;
            _Loan.FromTimestamp = loan.FromTimestamp.Date;
            _Loan.ExpiryTimestamp = loan.ExpiryTimestamp.Date.AddDays(1).AddSeconds(-1);
            _Loan.AmendedTimestamp = DateTime.Now;

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }
    }
}