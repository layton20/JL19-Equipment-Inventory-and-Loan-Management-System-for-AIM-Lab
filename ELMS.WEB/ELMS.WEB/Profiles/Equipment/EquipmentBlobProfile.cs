using AutoMapper;
using ELMS.WEB.Areas.General.Models.Media;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;

namespace ELMS.WEB.Profiles.Equipment
{
    public class EquipmentBlobProfile : Profile
    {
        public EquipmentBlobProfile()
        {
            CreateMap<CreateEquipmentBlobRequest, EquipmentBlobEntity>();
            CreateMap<EquipmentBlobEntity, EquipmentBlobResponse>().ReverseMap();
            CreateMap<EquipmentBlobResponse, EquipmentMediaViewModel>();
        }
    }
}