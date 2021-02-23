using AutoMapper;
using ELMS.WEB.Areas.Loan.Models.LoanExtension;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;

namespace ELMS.WEB.Profiles.Loan
{
    public class LoanExtensionProfile : Profile
    {
        public LoanExtensionProfile()
        {
            CreateMap<CreateLoanExtensionRequest, LoanExtensionEntity>();
            CreateMap<CreateLoanExtensionViewModel, CreateLoanExtensionRequest>();
            CreateMap<UpdateLoanExtensionRequest, LoanExtensionEntity>();
            CreateMap<LoanExtensionEntity, LoanExtensionResponse>();
            CreateMap<LoanExtensionResponse, LoanExtensionViewModel>();
        }
    }
}
