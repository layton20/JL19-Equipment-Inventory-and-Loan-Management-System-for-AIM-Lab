using AutoMapper;
using ELMS.WEB.Areas.Report.Models;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;

namespace ELMS.WEB.Profiles.Report
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<LoanResponse, LoanHistoryItemViewModel>();
            CreateMap<EquipmentResponse, EquipmentValueReportItemViewModel>();
        }
    }
}