using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;
using ELMS.WEB.Models.Base.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Admin.Interfaces
{
    public interface IConfigurationManager
    {
        public Task<ConfigurationResponse> CreateAsync(CreateConfigurationRequest request);
        public Task<BaseResponse> DeleteAsync(Guid uid);
        public Task<IList<ConfigurationResponse>> GetAsync();
        public Task<ConfigurationResponse> GetByUIDAsync(Guid uid);
        public Task<BaseResponse> UpdateAsync(UpdateConfigurationRequest request);
    }
}
