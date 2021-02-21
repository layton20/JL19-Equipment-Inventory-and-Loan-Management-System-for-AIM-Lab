using AutoMapper;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Models.Loan.Response;

namespace ELMS.WEB.Profiles.Loan
{
    public class LoanEquipmentProfile : Profile
    {
        public LoanEquipmentProfile()
        {
            CreateMap<LoanEquipmentEntity, LoanEquipmentResponse>();
        }
    }
}
