using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;

namespace ELMS.WEB.Profiles.Equipment
{
    public class EquipmentProfile : Profile
    {
        public EquipmentProfile()
        {
            CreateMap<EquipmentResponse, EquipmentViewModel>().ReverseMap();
            CreateMap<EquipmentViewModel, EquipmentEntity>();
            CreateMap<EquipmentEntity, EquipmentResponse>().ReverseMap();
            CreateMap<CreateEquipmentViewModel, CreateEquipmentRequest>();
            CreateMap<CreateEquipmentRequest, EquipmentEntity>();
            CreateMap<EquipmentResponse, UpdateEquipmentViewModel>();
        }
    }
}
