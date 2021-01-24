using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Concrete
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationContext __Context;

        public NoteRepository(ApplicationContext context)
        {
            __Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<NoteEntity> CreateAsync(NoteEntity note)
        {
            if (note == null || note.UID == Guid.Empty)
            {
                return null;
            }

            await __Context.Notes.AddAsync(note);
            bool _Added = await __Context.SaveChangesAsync() > 0;

            return _Added ? note : null;
        }

        public async Task<bool> DeleteAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return false;
            }

            NoteEntity _Note = await __Context.Notes.FirstOrDefaultAsync(x => x.UID == uid);

            if (_Note == null)
            {
                return false;
            }

            __Context.Notes.Remove(_Note);

            return await __Context.SaveChangesAsync() > 0;
        }

        public async Task<IList<NoteEntity>> GetAsync(Guid equipmentUID)
        {
            EquipmentEntity _Equipment = await __Context.Equipment.FirstOrDefaultAsync(x => x.UID == equipmentUID);

            if (_Equipment == null)
            {
                return Enumerable.Empty<NoteEntity>().ToList();
            }

            return await __Context.Notes.Where(x => x.EquipmentUID == equipmentUID).ToListAsync();
        }

        public async Task<NoteEntity> GetByUIDAsync(Guid uid)
        {
            return await __Context.Notes.FirstOrDefaultAsync(x => x.UID == uid);
        }

        public async Task<bool> UpdateAsync(NoteEntity note)
        {
            if (note.UID == Guid.Empty)
            {
                return false;
            }

            NoteEntity _Note = await __Context.Notes.FirstOrDefaultAsync(x => x.UID == note.UID);

            if (_Note == null)
            {
                return false;
            }

            _Note.Name = note.Name;
            _Note.Description = note.Description;
            _Note.AmendedTimestamp = DateTime.Now;

            return await __Context.SaveChangesAsync() > 0;
        }
    }
}
