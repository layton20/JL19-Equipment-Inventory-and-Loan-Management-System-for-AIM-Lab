using AutoMapper;
using ELMS.WEB.Entities.General;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.General.Request;
using ELMS.WEB.Models.General.Response;
using ELMS.WEB.Repositories.General.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.General.Concrete
{
    public class BlobManager : IBlobManager
    {
        private readonly IMapper __Mapper;
        private readonly IBlobRepository __BlobRepository;
        private readonly string MODEL_NAME = "Blob";

        public BlobManager(IMapper mapper, IBlobRepository blobRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __BlobRepository = blobRepository ?? throw new ArgumentNullException(nameof(blobRepository));
        }

        public async Task<BlobResponse> CreateAsync(CreateBlobRequest request)
        {
            BlobResponse _Response = __Mapper.Map<BlobResponse>(await __BlobRepository.CreateAsync(__Mapper.Map<BlobEntity>(request)));

            if (_Response == null)
            {
                _Response = new BlobResponse();
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __BlobRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<BlobResponse>> GetAsync()
        {
            return __Mapper.Map<IList<BlobResponse>>(await __BlobRepository.GetAsync());
        }

        public async Task<BlobResponse> GetAsync(Guid uid)
        {
            BlobResponse _Response = __Mapper.Map<BlobResponse>(await __BlobRepository.GetAsync(uid));

            if (_Response == null)
            {
                return new BlobResponse
                {
                    Success = false,
                    Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} get {MODEL_NAME}."
                };
            }

            return _Response;
        }

        public async Task<BaseResponse> UpdateAsync(UpdateBlobRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (request.UID == Guid.Empty || !await __BlobRepository.UpdateAsync(__Mapper.Map<BlobEntity>(request)))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME} details.";
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {MODEL_NAME} details.";
            }

            return _Response;
        }
    }
}