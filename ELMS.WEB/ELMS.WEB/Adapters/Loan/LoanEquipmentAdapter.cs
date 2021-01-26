using ELMS.WEB.Areas.Loan.Models;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Models.Loan.Response;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Loan
{
    internal static class LoanEquipmentAdapter
    {
        internal static LoanEquipmentResponse ToResponse(this LoanEquipmentEntity entity)
        {
            return entity == null ? null : new LoanEquipmentResponse
            {
                UID = entity.UID,
                EquipmentUID = entity.EquipmentUID,
                LoanUID = entity.LoanUID,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp
            };
        }

        internal static IList<LoanEquipmentResponse> ToResponse(this IList<LoanEquipmentEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(ToResponse).ToList() : Enumerable.Empty<LoanEquipmentResponse>().ToList();
        }

        internal static LoanEquipmentViewModel ToViewModel(this LoanEquipmentResponse response)
        {
            return response == null ? null : new LoanEquipmentViewModel
            {
                UID = response.UID,
                CreatedTimestamp = response.CreatedTimestamp,
                AmendedTimestamp = response.AmendedTimestamp
            };
        }

        internal static IList<LoanEquipmentViewModel> ToViewModel(this IList<LoanEquipmentResponse> responses)
        {
            return responses != null && responses.Count > 0 ? responses.Select(ToViewModel).ToList() : Enumerable.Empty<LoanEquipmentViewModel>().ToList();
        }
    }
}
