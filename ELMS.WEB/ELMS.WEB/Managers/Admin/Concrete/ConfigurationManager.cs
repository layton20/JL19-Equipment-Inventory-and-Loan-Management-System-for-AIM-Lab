using AutoMapper;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Repositories.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Admin.Concrete
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IMapper __Mapper;
        private readonly IConfigurationRepository __ConfigurationRepository;
        private const string ENTITY_NAME = "Configuration";

        public ConfigurationManager(IMapper mapper, IConfigurationRepository configurationRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __ConfigurationRepository = configurationRepository ?? throw new ArgumentNullException(nameof(configurationRepository));
        }

        public async Task<ConfigurationResponse> CreateAsync(CreateConfigurationRequest request)
        {
            ConfigurationResponse _Response = __Mapper.Map<ConfigurationResponse>(await __ConfigurationRepository.CreateAsync(__Mapper.Map<ConfigurationEntity>(request)));

            if (_Response == null)
            {
                return new ConfigurationResponse
                {
                    Success = false,
                    Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {ENTITY_NAME}."
                };
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}";
            }

            return _Response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __ConfigurationRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}.";
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}.";
            }

            return _Response;
        }

        public async Task<IList<ConfigurationResponse>> GetAsync()
        {
            return __Mapper.Map<IList<ConfigurationResponse>>(await __ConfigurationRepository.GetAsync());
        }

        public async Task<ConfigurationResponse> GetByUIDAsync(Guid uid)
        {
            return __Mapper.Map<ConfigurationResponse>(await __ConfigurationRepository.GetByUIDAsync(uid));
        }

        public async Task<BaseResponse> UpdateAsync(UpdateConfigurationRequest request)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __ConfigurationRepository.UpdateAsync(__Mapper.Map<ConfigurationEntity>(request)))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME}.";
            }
            else
            {
                _Response.Message = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {ENTITY_NAME}.";
            }

            return _Response;
        }

        public async Task<ConfigurationResponse> GetByNormalizedNameAsync(string name)
        {
            return __Mapper.Map<ConfigurationResponse>(await __ConfigurationRepository.GetByNormalizedNameAsync(name));
        }
    }
}
