using ELMS.WEB.Areas.Loan.Models;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Loan
{
    internal static class LoanAdapter
    {
        internal static LoanEntity ToEntity(this CreateLoanRequest request)
        {
            return request == null ? null : new LoanEntity
            {
                Name = request.Name,
                LoaneeEmail = request.LoaneeEmailAddress,
                LoanerEmail = request.LoanerEmailAddress,
                FromTimestamp = request.FromTimestamp,
                ExpiryTimestamp = request.ExpiryTimestamp,
                AcceptedTermsAndConditions = request.AcceptedTermsAndConditions,
                Status = request.Status
            };
        }

        internal static LoanEntity ToEntity(this UpdateLoanRequest request)
        {
            return request == null ? null : new LoanEntity
            {
                UID = request.UID,
                Name = request.Name,
                FromTimestamp = request.FromTimestamp,
                ExpiryTimestamp = request.ExpiryTimestamp,
                AcceptedTermsAndConditions = request.AcceptedTermsAndConditions,
                Status = request.Status
            };
        }

        internal static LoanResponse ToResponse(this LoanEntity entity)
        {
            return entity == null ? null : new LoanResponse
            {
                UID = entity.UID,
                Name = entity.Name,
                LoanerEmail = entity.LoanerEmail,
                LoaneeEmail = entity.LoaneeEmail,
                AcceptedTermsAndConditions = entity.AcceptedTermsAndConditions,
                FromTimestamp = entity.FromTimestamp,
                ExpiryTimestamp = entity.ExpiryTimestamp,
                Status = entity.Status,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
            };
        }

        internal static IList<LoanResponse> ToResponse(this IList<LoanEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(ToResponse).ToList() : Enumerable.Empty<LoanResponse>().ToList();
        }

        internal static CreateLoanRequest ToRequest(this CreateLoanViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            CreateLoanRequest _Request = new CreateLoanRequest
            {
                LoaneeEmailAddress = model.LoaneeEmailAddress,
                Name = model.Name,
                FromTimestamp = model.FromTimestamp,
                ExpiryTimestamp = model.ExpiryTimestamp,
                EquipmentList = model.SelectedEquipment
            };

            return _Request;
        }

        internal static LoanViewModel ToViewModel(this LoanResponse response)
        {
            return response == null ? null : new LoanViewModel
            {
                UID = response.UID,
                Name = response.Name,
                LoaneeEmail = response.LoaneeEmail,
                StartTimestamp = response.FromTimestamp,
                ExpiryTimestamp = response.ExpiryTimestamp,
                Status = response.Status,
                AcceptedTermsAndConditions = response.AcceptedTermsAndConditions,
                CreatedTimestamp = response.CreatedTimestamp,
                AmendedTimestamp = response.AmendedTimestamp,
                LoanerEmail = response.LoanerEmail
            };
        }

        internal static IList<LoanViewModel> ToViewModel(this IList<LoanResponse> responses)
        {
            return responses != null && responses.Count > 0 ? responses.Select(ToViewModel).ToList() : Enumerable.Empty<LoanViewModel>().ToList();
        }

        internal static CreateLoanRequest ToRequest(this ConfirmationLoanViewModel model)
        {
            return model == null ? null : new CreateLoanRequest
            {
                Name = model.Name,
                EquipmentList = model.SelectedEquipment,
                ExpiryTimestamp = model.ExpiryTimestamp,
                LoaneeEmailAddress = model.LoaneeEmailAddress,
                FromTimestamp = model.FromTimestamp,
                LoanerEmailAddress = model.LoanerEmailAddress
            };
        }
    }
}