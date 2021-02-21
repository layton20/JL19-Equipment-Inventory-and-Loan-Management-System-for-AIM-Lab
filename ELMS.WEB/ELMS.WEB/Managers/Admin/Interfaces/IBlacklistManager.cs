using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;
using ELMS.WEB.Models.Base.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Admin.Interfaces
{
    public interface IBlacklistManager
    {
        Task<BlacklistResponse> CreateAsync(CreateBlacklistRequest request);

        Task<BaseResponse> UpdateAsync(UpdateBlacklistRequest request);

        Task<IList<BlacklistResponse>> GetAsync(string email);

        Task<IList<BlacklistResponse>> GetAsync();

        Task<BlacklistResponse> GetByUIDAsync(Guid uid);

        Task<BaseResponse> DeleteAsync(Guid uid);
    }
}