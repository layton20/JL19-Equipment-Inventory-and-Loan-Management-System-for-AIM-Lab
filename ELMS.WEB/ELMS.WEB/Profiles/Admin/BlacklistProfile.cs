using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Blacklist;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;

namespace ELMS.WEB.Profiles.Admin
{
    public class BlacklistProfile : Profile
    {
        public BlacklistProfile()
        {
            CreateMap<BlacklistResponse, DeleteViewModel>();
            CreateMap<UpdateViewModel, UpdateBlacklistRequest>();
            CreateMap<CreateViewModel, CreateBlacklistRequest>();
            CreateMap<BlacklistResponse, BlacklistViewModel>();
            CreateMap<UpdateBlacklistRequest, BlacklistEntity>();
            CreateMap<CreateBlacklistRequest, BlacklistEntity>();
            CreateMap<BlacklistEntity, BlacklistResponse>().ReverseMap();
        }
    }
}