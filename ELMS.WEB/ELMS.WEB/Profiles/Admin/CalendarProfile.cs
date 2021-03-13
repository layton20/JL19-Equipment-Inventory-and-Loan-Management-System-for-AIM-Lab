using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Calendar;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;
using System.Linq;

namespace ELMS.WEB.Profiles.Admin
{
    public class CalendarProfile : Profile
    {
        public CalendarProfile()
        {
            CreateMap<LoanResponse, CalendarItemLoanViewModel>()
                .ForMember(dest =>
                    dest.ReferenceUID,
                    opt => opt.MapFrom(src => src.UID))
                .ForMember(dest =>
                    dest.LoaneeEmail,
                    opt => opt.MapFrom(src => src.LoaneeEmail))
                .ForMember(dest =>
                    dest.EquipmentNames,
                    opt => opt.MapFrom(src => src.EquipmentList.Select(x => x.Name)))
                .ForMember(dest =>
                    dest.StartTimestamp,
                    opt => opt.MapFrom(src => src.FromTimestamp))
                .ForMember(dest =>
                    dest.EndTimestamp,
                    opt => opt.MapFrom(src => src.ExpiryTimestamp));

            CreateMap<EquipmentResponse, CalendarItemEquipmentViewModel>()
                .ForMember(dest =>
                    dest.ReferenceUID,
                    opt => opt.MapFrom(src => src.UID))
                .ForMember(dest =>
                    dest.EndTimestamp,
                    opt => opt.MapFrom(src => src.WarrantyExpirationDate))
                .ForMember(dest =>
                    dest.StartTimestamp,
                    opt => opt.MapFrom(src => src.PurchaseDate));
        }
    }
}
