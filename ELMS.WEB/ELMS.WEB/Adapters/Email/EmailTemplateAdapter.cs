using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Email
{
    internal static class EmailTemplateAdapter
    {
        internal static EmailTemplateEntity ToEntity(this CreateEmailTemplateRequest request)
        {
            return request == null ? null : new EmailTemplateEntity
            {
                Name = request.Name,
                Body = request.Body,
                Subject = request.Subject,
                OwnerUID = request.OwnerUID,
                TemplateType = request.TemplateType
            };
        }

        internal static EmailTemplateEntity ToEntity(this UpdateEmailTemplateRequest request)
        {
            return request == null ? null : new EmailTemplateEntity
            {
                UID = request.UID,
                Name = request.Name,
                Body = request.Body,
                Subject = request.Subject,
                TemplateType = request.TemplateType
            };
        }

        internal static EmailTemplateResponse ToResponse(this EmailTemplateEntity entity)
        {
            return entity == null ? null : new EmailTemplateResponse
            {
                UID = entity.UID,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
                Name = entity.Name,
                Body = entity.Body,
                OwnerUID = entity.OwnerUID,
                Subject = entity.Subject,
                TemplateType = entity.TemplateType
            };
        }

        internal static IList<EmailTemplateResponse> ToResponse(this IList<EmailTemplateEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(ToResponse).ToList() : Enumerable.Empty<EmailTemplateResponse>().ToList();
        }

        internal static CreateEmailTemplateRequest ToRequest(this CreateEmailTemplateViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            CreateEmailTemplateRequest _Request = new CreateEmailTemplateRequest
            {
                Name = model.Name,
                Subject = model.Subject,
                Body = model.Body,
                TemplateType = model.TemplateType
            };

            if (Guid.TryParse(model.OwnerUID, out Guid ownerUID))
            {
                _Request.OwnerUID = ownerUID;
            }

            return _Request;
        }

        internal static EmailTemplateViewModel ToViewModel(this EmailTemplateResponse response)
        {
            return response == null ? null : new EmailTemplateViewModel
            {
                UID = response.UID,
                Name = response.Name,
                Subject = response.Subject,
                Body = response.Body,
                OwnerUID = response.OwnerUID,
                TemplateType = response.TemplateType,
                CreatedTimestamp = response.CreatedTimestamp,
                AmendedTimestamp = response.AmendedTimestamp
            };
        }

        internal static IList<EmailTemplateViewModel> ToViewModel(this IList<EmailTemplateResponse> responses)
        {
            return responses != null && responses.Count > 0 ? responses.Select(ToViewModel).ToList() : Enumerable.Empty<EmailTemplateViewModel>().ToList();
        }

        internal static UpdateEmailTemplateRequest ToRequest(this EmailTemplateViewModel model)
        {
            return model == null ? null : new UpdateEmailTemplateRequest
            {
                UID = model.UID,
                Subject = model.Subject,
                Name = model.Name,
                Body = model.Body,
                TemplateType = model.TemplateType
            };
        }
    }
}