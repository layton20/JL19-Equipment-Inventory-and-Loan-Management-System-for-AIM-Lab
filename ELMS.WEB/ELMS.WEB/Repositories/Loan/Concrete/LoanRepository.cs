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

            await __ApplicationContext.Loans.AddAsync(loan);
            await __ApplicationContext.LoanEquipmentList.AddRangeAsync(_EquipmentList.Select(x => new LoanEquipmentEntity
            {
                EquipmentUID = x.UID,
                LoanUID = loan.UID
            }).ToList());

            bool _Added = await __ApplicationContext.SaveChangesAsync() > 0;
            return loan;
        }

        public async Task<IList<LoanEntity>> GetAsync(Guid equipmentUID, bool all = false)
        {
            if (all)
            {
                return await __ApplicationContext.Loans.ToListAsync();
            }

            return await __ApplicationContext.Loans.Where(x => x.Status == Status.OnLoan).ToListAsync();
        }

        public async Task<IList<LoanEntity>> GetAsync()
        {
            return await __ApplicationContext.Loans.ToListAsync();
        }

        public async Task<LoanEntity> GetByUIDAsync(Guid uid)
        {
            return await __ApplicationContext.Loans.FirstOrDefaultAsync(x => x.UID == uid);
        }

        public async Task<int> GetCountByStatus(Status status)
        {
            return await __ApplicationContext.Loans.Where(x => x.Status == status).CountAsync();
        }

        public async Task<bool> UpdateAsync(LoanEntity loan, IList<Guid> updatedEquipmentList)
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

            _Loan.Name = loan.Name;
            _Loan.Status = loan.Status;
            _Loan.AmendedTimestamp = DateTime.Now;
            _Loan.AcceptedTermsAndConditions = _Loan.AcceptedTermsAndConditions;
            _Loan.FromTimestamp = _Loan.FromTimestamp;
            _Loan.ExpiryTimestamp = _Loan.ExpiryTimestamp;

            IList<Guid> _UpdatedValidEquipmentList = await __ApplicationContext.Equipment.Where(x => updatedEquipmentList.Contains(x.UID)).Select(x => x.UID).ToListAsync();
            IList<Guid> _CurrentEquipmentList = await __ApplicationContext.LoanEquipmentList.Where(x => x.LoanUID == loan.UID).Select(x => x.EquipmentUID).ToListAsync();

            // Entities to Delete = Equipment in the current list, but not in the new list
            IList<Guid> _ToDelete = _CurrentEquipmentList.Where(uid => !_UpdatedValidEquipmentList.Contains(uid)).ToList();
            IList<LoanEquipmentEntity> _ToDeleteEntities = await __ApplicationContext.LoanEquipmentList.Where(x => _ToDelete.Contains(x.EquipmentUID)).ToListAsync();

            // Entities to Add = Equipment in the new list, but not in the current list
            IList<Guid> _ToAdd = _UpdatedValidEquipmentList.Where(uid => !_CurrentEquipmentList.Contains(uid)).ToList();
            IList<LoanEquipmentEntity> _ToAddEntities = _ToAdd.Select(equipmentUID => new LoanEquipmentEntity
            {
                LoanUID = loan.UID,
                EquipmentUID = equipmentUID,
            }).ToList();

            __ApplicationContext.LoanEquipmentList.RemoveRange(_ToDeleteEntities);
            await __ApplicationContext.LoanEquipmentList.AddRangeAsync(_ToAddEntities);

            return await __ApplicationContext.SaveChangesAsync() > 0;
        }
    }
}