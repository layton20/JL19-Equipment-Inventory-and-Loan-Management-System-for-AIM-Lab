using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Interfaces
{
    public interface INoteManager
    {
        public Task<NoteResponse> CreateAsync(CreateNoteRequest request);

        public Task<NoteResponse> GetByUIDAsync(Guid uid);

        public Task<IList<NoteResponse>> GetAsync(Guid equipmentUID);

        public Task<BaseResponse> UpdateAsync(UpdateNoteRequest request);

        public Task<BaseResponse> DeleteAsync(Guid uid);
    }
}