using ELMS.WEB.Areas.Admin.Models.Blacklist;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Admin
{
    internal static class BlacklistAdapter
    {
        internal static BlacklistEntity ToEntity(this CreateBlacklistRequest request)
        {
            return request == null ? null : new BlacklistEntity
            {
                Email = request.Email,
                Reason = request.Reason,
                Type = request.Type,
                Active = request.Active
            };
        }

        internal static BlacklistResponse ToResponse(this BlacklistEntity entity)
        {
            return entity == null ? null : new BlacklistResponse
            {
                UID = entity.UID,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
                Email = entity.Email,
                Reason = entity.Reason,
                Type = entity.Type,
                Active = entity.Active
            };
        }

        internal static IList<BlacklistResponse> ToResponse(this IList<BlacklistEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(ToResponse).ToList() : Enumerable.Empty<BlacklistResponse>().ToList();
        }

        internal static BlacklistEntity ToEntity(this UpdateBlacklistRequest request)
        {
            return request == null ? null : new BlacklistEntity
            {
                Reason = request.Reason,
                UID = request.UID,
                Active = request.Active,
                Type = request.Type
            };
        }

        internal static BlacklistViewModel ToViewModel(this BlacklistResponse response)
        {
            return response == null ? null : new BlacklistViewModel
            {
                UID = response.UID,
                Email = response.Email,
                Reason = response.Reason,
                Type = response.Type,
                Active = response.Active,
                AmendedTimestamp = response.AmendedTimestamp,
                CreatedTimestamp = response.CreatedTimestamp
            };
        }

        internal static IList<BlacklistViewModel> ToViewModel(this IList<BlacklistResponse> responses)
        {
            return responses != null && responses.Count > 0 ? responses.Select(ToViewModel).ToList() : Enumerable.Empty<BlacklistViewModel>().ToList();
        }

        internal static CreateBlacklistRequest ToRequest(this CreateViewModel model)
        {
            return model == null ? null : new CreateBlacklistRequest
            {
                Email = model.Email,
                Active = model.Active,
                Reason = model.Reason,
                Type = model.Type
            };
        }

        internal static UpdateBlacklistRequest ToRequest(this UpdateBlacklistViewModel model)
        {
            return model == null ? null : new UpdateBlacklistRequest
            {
                Active = model.Active,
                Reason = model.Reason,
                UID = model.UID,
                Type = model.Type
            };
        }

        internal static DeleteBlacklistViewModel ToDeleteViewModel(this BlacklistResponse response)
        {
            return response == null ? null : new DeleteBlacklistViewModel
            {
                UID = response.UID,
                Email = response.Email,
                Reason = response.Reason,
                Active = response.Active,
                Type = response.Type,
            };
        }
    }
}
