using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Configuration;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;

namespace ELMS.WEB.Profiles.Admin
{
    public class ConfigurationProfile : Profile
    {
        public ConfigurationProfile()
        {
            CreateMap<ConfigurationEntity, ConfigurationResponse>();
            CreateMap<CreateConfigurationRequest, ConfigurationEntity>();
            CreateMap<UpdateConfigurationRequest, ConfigurationEntity>();
            CreateMap<CreateConfigurationViewModel, CreateConfigurationRequest>();
            CreateMap<UpdateConfigurationViewModel, UpdateConfigurationRequest>();
            CreateMap<ConfigurationResponse, ConfigurationViewModel>();
            CreateMap<ConfigurationResponse, UpdateConfigurationViewModel>();
        }
    }
}