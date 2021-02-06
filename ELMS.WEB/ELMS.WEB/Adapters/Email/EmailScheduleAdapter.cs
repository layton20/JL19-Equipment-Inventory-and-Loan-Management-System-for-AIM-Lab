using ELMS.WEB.Areas.Email.Models.EmailSchedule;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Email
{
    internal static class EmailScheduleAdapter
    {
        internal static EmailScheduleEntity ToEntity(this CreateEmailScheduleRequest request)
        {
            return request == null ? null : new EmailScheduleEntity
            {
                EmailTemplateUID = request.EmailTemplateUID,
                RecipientEmailAddress = request.RecipientEmailAddress,
                EmailType = request.EmailType,
                SendTimestamp = request.SendTimestamp
            };
        }

        internal static EmailScheduleResponse ToResponse(this EmailScheduleEntity entity)
        {
            return entity == null ? null : new EmailScheduleResponse
            {
                UID = entity.UID,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
                EmailTemplateUID = entity.EmailTemplateUID,
                RecipientEmailAddress = entity.RecipientEmailAddress,
                EmailType = entity.EmailType,
                Sent = entity.Sent,
                SendTimestamp = entity.SendTimestamp
            };
        }

        internal static IList<EmailScheduleResponse> ToResponse(this IList<EmailScheduleEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(ToResponse).ToList() : Enumerable.Empty<EmailScheduleResponse>().ToList();
        }

        internal static EmailScheduleViewModel ToViewModel(this EmailScheduleResponse response)
        {
            return response == null ? null : new EmailScheduleViewModel
            {
                UID = response.UID,
                CreatedTimestamp = response.CreatedTimestamp,
                AmendedTimestamp = response.AmendedTimestamp,
                EmailTemplateUID = response.EmailTemplateUID,
                RecipientEmail = response.RecipientEmailAddress,
                EmailSent = response.Sent,
                SendTimestamp = response.SendTimestamp,
                EmailType = response.EmailType
            };
        }

        internal static IList<EmailScheduleViewModel> ToViewModel(this IList<EmailScheduleResponse> responses)
        {
            return responses != null && responses.Count > 0 ? responses.Select(ToViewModel)?.ToList() : Enumerable.Empty<EmailScheduleViewModel>().ToList();
        }
    }
}
