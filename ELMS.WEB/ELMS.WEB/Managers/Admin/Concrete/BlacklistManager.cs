using ELMS.WEB.Adapters.Admin;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Repositories.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Admin.Concrete
{
    public class BlacklistManager : IBlacklistManager
    {
        private readonly IBlacklistRepository __BlacklistRepository;
        private const string MODEL_NAME = "Blacklist";

        public BlacklistManager(IBlacklistRepository blacklistRepository)
        {
            __BlacklistRepository = blacklistRepository ?? throw new ArgumentNullException(nameof(blacklistRepository));
        }

        public async Task<BlacklistResponse> CreateAsync(CreateBlacklistRequest request)
        {
            BlacklistResponse _Response = (await __BlacklistRepository.CreateAsync(request.ToEntity())).ToResponse();

            if (_Response == null)
            {
                return new BlacklistResponse
                {
                    Success = false,
                    Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}."
                };
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {MODEL_NAME}";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __BlacklistRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {MODEL_NAME}.";
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<BlacklistResponse>> GetAsync(string email)
        {
            return (await __BlacklistRepository.GetAsync(email)).ToResponse();
        }

        public async Task<BaseResponse> UpdateAsync(UpdateBlacklistRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __BlacklistRepository.UpdateAsync(request.ToEntity()))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME}.";
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {MODEL_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<BlacklistResponse>> GetAsync()
        {
            return (await __BlacklistRepository.GetAsync()).ToResponse();
        }

        public async Task<BlacklistResponse> GetByUIDAsync(Guid uid)
        {
            return (await __BlacklistRepository.GetByUIDAsync(uid)).ToResponse();
        }
    }
}
