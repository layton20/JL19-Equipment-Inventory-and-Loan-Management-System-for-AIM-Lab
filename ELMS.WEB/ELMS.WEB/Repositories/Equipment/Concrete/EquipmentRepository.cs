using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Concrete
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly ApplicationContext __Context;

        public EquipmentRepository(ApplicationContext context)
        {
            __Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<EquipmentEntity> CreateAsync(EquipmentEntity equipment)
        {
            if (equipment == null || equipment.UID == Guid.Empty)
            {
                return null;
            }

            equipment.WarrantyExpirationDate = equipment.WarrantyExpirationDate.Date.AddDays(1).AddSeconds(-1);
            await __Context.Equipment.AddAsync(equipment);
            bool _Added = await __Context.SaveChangesAsync() > 0;

            return _Added ? equipment : null;
        }

        public async Task<IList<EquipmentEntity>> BulkCreateAsync(EquipmentEntity equipment, int Quantity)
        {
            if (equipment == null || equipment.UID == Guid.Empty)
            {
                return null;
            }

            List<EquipmentEntity> _Copies = new List<EquipmentEntity>();

            for (int i = 0; i < Quantity; i++)
            {
                _Copies.Add(new EquipmentEntity
                {
                    Name = $"{equipment.Name} ({i + 1})",
                    Description = equipment.Description,
                    WarrantyExpirationDate = equipment.WarrantyExpirationDate.Date.AddDays(1).AddSeconds(-1),
                    Status = equipment.Status,
                    PurchaseDate = equipment.PurchaseDate,
                    PurchasePrice = equipment.PurchasePrice,
                    SerialNumber = equipment.SerialNumber
                });
            }

            await __Context.Equipment.AddRangeAsync(_Copies);

            if (await __Context.SaveChangesAsync() <= 0)
            {
                return null;
            }

            return _Copies;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            EquipmentEntity _Equipment = await __Context.Equipment.FirstOrDefaultAsync(x => x.UID == uid);

            if (_Equipment == null)
            {
                return false;
            }

            __Context.Equipment.Remove(_Equipment);

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<EquipmentEntity>> GetAsync()
        {
            return await __Context.Equipment.OrderByDescending(x => x.CreatedTimestamp).ToListAsync();
        }

        public async Task<EquipmentEntity> GetAsync(Guid uid)
        {
            return await __Context.Equipment.FirstOrDefaultAsync(x => x.UID == uid);
        }

        public async Task<bool> UpdateAsync(EquipmentEntity equipment)
        {
            if (equipment.UID == Guid.Empty)
            {
                return false;
            }

            EquipmentEntity _Equipment = await __Context.Equipment.FirstOrDefaultAsync(x => x.UID == equipment.UID);

            if (_Equipment == null)
            {
                return false;
            }

            _Equipment.Name = equipment.Name;
            _Equipment.Description = equipment.Description;
            _Equipment.SerialNumber = equipment.SerialNumber;
            _Equipment.WarrantyExpirationDate = equipment.WarrantyExpirationDate.AddDays(1).AddSeconds(-1);
            _Equipment.PurchaseDate = equipment.PurchaseDate;
            _Equipment.PurchasePrice = equipment.PurchasePrice;
            _Equipment.ReplacementPrice = equipment.ReplacementPrice;
            _Equipment.Status = equipment.Status;

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IList<EquipmentEntity>> GetAsync(IList<Guid> uids)
        {
            return await __Context.Equipment.Where(x => uids.Contains(x.UID)).OrderByDescending(x => x.CreatedTimestamp).ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(Guid uid, Status status)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            EquipmentEntity _EquipmentEntity = await __Context.Equipment.FindAsync(uid);

            if (_EquipmentEntity == null)
            {
                return false;
            }

            _EquipmentEntity.Status = status;
            _EquipmentEntity.AmendedTimestamp = DateTime.Now;

            return await __Context.SaveChangesAsync() > 0;
        }
    }
}