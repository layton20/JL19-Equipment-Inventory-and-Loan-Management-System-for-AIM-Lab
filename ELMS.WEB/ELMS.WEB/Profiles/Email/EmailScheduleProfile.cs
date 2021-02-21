using AutoMapper;
using ELMS.WEB.Areas.Email.Models.EmailSchedule;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;

namespace ELMS.WEB.Profiles.Email
{
    public class EmailScheduleProfile : Profile
    {
        public EmailScheduleProfile()
        {
            CreateMap<EmailScheduleResponse, EmailScheduleViewModel>();
            CreateMap<EmailScheduleEntity, EmailScheduleResponse>();
            CreateMap<CreateEmailScheduleRequest, EmailScheduleEntity>();
        }
    }
}