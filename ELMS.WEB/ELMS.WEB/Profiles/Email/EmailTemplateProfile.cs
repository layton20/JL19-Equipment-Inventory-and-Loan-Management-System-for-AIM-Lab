using AutoMapper;
using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;

namespace ELMS.WEB.Profiles.Email
{
    public class EmailTemplateProfile : Profile
    {
        public EmailTemplateProfile()
        {
            CreateMap<EmailTemplateViewModel, UpdateEmailTemplateRequest>();
            CreateMap<EmailTemplateResponse, EmailTemplateViewModel>();
            CreateMap<EmailTemplateEntity, EmailTemplateResponse>();
            CreateMap<CreateEmailTemplateViewModel, CreateEmailTemplateRequest>();
            CreateMap<CreateEmailTemplateRequest, EmailTemplateEntity>();
            CreateMap<UpdateEmailTemplateRequest, EmailTemplateEntity>();
        }
    }
}