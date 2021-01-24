using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Equipment
{
    internal static class NoteAdapter
    {
        internal static NoteEntity ToEntity(this CreateNoteRequest request)
        {
            return request == null ? null : new NoteEntity
            {
                Name = request.Name,
                Description = request.Description,
                EquipmentUID = request.EquipmentUID,
                OwnerUID = request.OwnerUID
            };
        }

        internal static NoteEntity ToEntity(this UpdateNoteRequest request)
        {
            return request == null ? null : new NoteEntity
            {
                Name = request.Name,
                Description = request.Description
            };
        }

        internal static NoteResponse ToResponse(this NoteEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            NoteResponse _Response = new NoteResponse
            {
                UID = entity.UID,
                Name = entity.Name,
                Description = entity.Description,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
            };

            if (Guid.TryParse(entity.OwnerUID, out Guid ownerUID))
            {
                _Response.OwnerUID = ownerUID;
            }

            return _Response;
        }

        internal static IList<NoteResponse> ToResponse(this IList<NoteEntity> entities)
        {
            return entities != null || entities.Count > 0 ? entities.Select(x => x.ToResponse()).ToList() : Enumerable.Empty<NoteResponse>().ToList();
        }

        internal static CreateNoteRequest ToRequest(this CreateNoteViewModel model)
        {
            return model == null ? null : new CreateNoteRequest
            {
                Name = model.Name,
                Description = model.Description,
                EquipmentUID = model.EquipmentUID,
                OwnerUID = model.OwnerUID
            };
        }

        internal static NoteViewModel ToViewModel(this NoteResponse response)
        {
            return response == null ? null : new NoteViewModel
            {
                Name = response.Name,
                Description = response.Description,
                CreatedTimestamp = response.CreatedTimestamp,
                OwnerUID = response.OwnerUID.ToString()
            };
        }

        internal static IList<NoteViewModel> ToViewModel(this IList<NoteResponse> responses)
        {
            return responses != null || responses.Count > 0 ? responses.Select(x => x.ToViewModel()).ToList() : Enumerable.Empty<NoteViewModel>().ToList();
        }
    }
}
