using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Email
{
    internal static class EmailScheduleParameterAdapter
    {
        internal static EmailScheduleParameterEntity ToEntity(this CreateEmailScheduleParameterRequest request)
        {
            return request == null ? null : new EmailScheduleParameterEntity
            {
                EmailScheduleUID = request.EmailScheduleUID,
                Name = request.Name,
                Value = request.Value
            };
        }

        internal static EmailScheduleParameterResponse ToResponse(this EmailScheduleParameterEntity entity)
        {
            return entity == null ? null : new EmailScheduleParameterResponse
            {
                UID = entity.UID,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
                EmailScheduleUID = entity.EmailScheduleUID,
                Name = entity.Name,
                Value = entity.Value
            };
        }

        internal static IList<EmailScheduleParameterResponse> ToResponse(this IList<EmailScheduleParameterEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(ToResponse)?.ToList() : Enumerable.Empty<EmailScheduleParameterResponse>().ToList();
        }
    }
}