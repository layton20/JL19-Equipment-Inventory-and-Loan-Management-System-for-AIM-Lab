using ELMS.WEB.Entities.Equipment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Equipment.Interfaces
{
    public interface INoteRepository
    {
        public Task<NoteEntity> CreateAsync(NoteEntity note);
        public Task<NoteEntity> GetByUIDAsync(Guid uid);
        public Task<IList<NoteEntity>> GetAsync(Guid equipmentUID);
        public Task<bool> UpdateAsync(NoteEntity equipment);
        public Task<bool> DeleteAsync(Guid uid);
    }
}
