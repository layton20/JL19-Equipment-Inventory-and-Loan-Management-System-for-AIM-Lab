using AutoMapper;
using ELMS.WEB.Areas.Loan.Models;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;

namespace ELMS.WEB.Profiles.Loan
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<LoanEntity, LoanResponse>().ReverseMap();
            CreateMap<CreateLoanRequest, LoanEntity>();
            CreateMap<UpdateLoanRequest, LoanEntity>();
            CreateMap<ConfirmationLoanViewModel, CreateLoanRequest>();
            CreateMap<LoanResponse, LoanViewModel>();
        }
    }
}