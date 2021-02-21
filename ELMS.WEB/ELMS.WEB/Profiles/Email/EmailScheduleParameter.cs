using AutoMapper;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;

namespace ELMS.WEB.Profiles.Email
{
    public class EmailScheduleParameter : Profile
    {
        public EmailScheduleParameter()
        {
            CreateMap<EmailScheduleParameterEntity, EmailScheduleParameterResponse>();
            CreateMap<CreateEmailScheduleParameterRequest, EmailScheduleParameterEntity>();
        }
    }
}
