using AutoMapper;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Equipment.Concrete
{
    public class NoteManager : INoteManager
    {
        private readonly IMapper __Mapper;
        private readonly INoteRepository __NoteRepository;
        private const string MODEL_NAME = "Note";

        public NoteManager(IMapper mapper, INoteRepository noteRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __NoteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        }

        public async Task<NoteResponse> CreateAsync(CreateNoteRequest request)
        {
            NoteResponse _Response = __Mapper.Map<NoteResponse>(await __NoteRepository.CreateAsync(__Mapper.Map<NoteEntity>(request)));

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} create ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __NoteRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} delete ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<NoteResponse>> GetAsync(Guid equipmentUID)
        {
            return __Mapper.Map<IList<NoteResponse>>(await __NoteRepository.GetAsync(equipmentUID));
        }

        public async Task<NoteResponse> GetByUIDAsync(Guid uid)
        {
            NoteResponse _Response = __Mapper.Map<NoteResponse>(await __NoteRepository.GetByUIDAsync(uid));

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} get ${MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(UpdateNoteRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty || !await __NoteRepository.UpdateAsync(__Mapper.Map<NoteEntity>(request)))
            {
                _Response.Success = false;
                _Response.Message = $"Error: ${GlobalConstants.ERROR_ACTION_PREFIX} update ${MODEL_NAME}.";
            }

            return _Response;
        }
    }
}