using ELMS.WEB.Areas.Email.Models.EmailSchedule;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                SendTimestamp = request.SendTimestamp
            };
        }

        internal static EmailScheduleResponse ToResponse(this EmailScheduleEntity entity)
        {
            return entity == null ? null : new EmailScheduleResponse
            {
                UID = entity.UID,
                EmailTemplateUID = entity.EmailTemplateUID,
                RecipientEmailAddress = entity.RecipientEmailAddress,
                SendTimestamp = entity.SendTimestamp,
                Status = entity.Status,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp
            };
        }

        internal static IList<EmailScheduleResponse> ToResponse(this IList<EmailScheduleEntity> responses)
        {
            EmailScheduleResponse _Response = new EmailScheduleResponse();

            return responses != null && responses.Count > 0 ? responses.Select(ToResponse).ToList() : Enumerable.Empty<EmailScheduleResponse>().ToList();
        }

        internal static EmailScheduleEntity ToEntity(this UpdateEmailScheduleRequest request)
        {
            return request == null ? null : new EmailScheduleEntity
            {
                UID = request.UID,
                EmailTemplateUID = request.EmailTemplateUID,
                Status = request.Status,
                SendTimestamp = request.SendTimestamp,
                RecipientEmailAddress = request.RecipientEmailAddress
            };
        }

        internal static CreateEmailScheduleRequest ToRequest(this CreateEmailScheduleViewModel model)
        {
            return model == null ? null : new CreateEmailScheduleRequest
            {
                EmailTemplateUID = model.SelectedTemplateUID,
                RecipientEmailAddress = model.RecipientEmailAddress,
                SendTimestamp = model.ScheduleTime
            };
        }

        internal static EmailScheduleViewModel ToViewModel(this EmailScheduleResponse response)
        {
            return response == null ? null : new EmailScheduleViewModel
            {
                UID = response.UID,
                EmailTemplateUID = response.EmailTemplateUID,
                RecipientEmailAddress = response.RecipientEmailAddress,
                SenderUID = response.SenderUID,
                SendTimestamp = response.SendTimestamp,
                Status = response.Status,
                CreatedTimestamp = response.CreatedTimestamp,
                AmendedTimestamp = response.AmendedTimestamp
            };
        }

        internal static IList<EmailScheduleViewModel> ToViewModel(this IList<EmailScheduleResponse> response)
        {
            return response != null && response.Count > 0 ? response.Select(ToViewModel).ToList() : Enumerable.Empty<EmailScheduleViewModel>().ToList();
        }
    }
}
