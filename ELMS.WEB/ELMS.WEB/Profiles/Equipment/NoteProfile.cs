using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;

namespace ELMS.WEB.Profiles.Equipment
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<CreateNoteRequest, NoteEntity>();
            CreateMap<NoteResponse, NoteEntity>().ReverseMap();
            CreateMap<UpdateNoteRequest, NoteEntity>();
            CreateMap<NoteViewModel, UpdateNoteRequest>();
            CreateMap<NoteResponse, NoteViewModel>();
            CreateMap<CreateNoteViewModel, CreateNoteRequest>();
        }
    }
}