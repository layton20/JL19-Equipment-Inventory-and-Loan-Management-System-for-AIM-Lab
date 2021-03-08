using AutoMapper;
using ELMS.WEB.Areas.General.Models.Media;
using ELMS.WEB.Entities.General;
using ELMS.WEB.Models.General.Request;
using ELMS.WEB.Models.General.Response;

namespace ELMS.WEB.Profiles.General
{
    public class BlobProfile : Profile
    {
        public BlobProfile()
        {
            CreateMap<CreateBlobRequest, BlobEntity>();
            CreateMap<UpdateBlobRequest, BlobEntity>();
            CreateMap<BlobEntity, BlobResponse>().ReverseMap();
            CreateMap<CreateEquipmentMediaViewModel, CreateBlobRequest>();
            CreateMap<BlobResponse, MediaViewModel>();
        }
    }
}