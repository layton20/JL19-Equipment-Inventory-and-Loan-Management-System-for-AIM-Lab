using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.General.Request;
using ELMS.WEB.Models.General.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.General.Interface
{
    public interface IBlobManager
    {
        Task<BlobResponse> CreateAsync(CreateBlobRequest request);
        Task<IList<BlobResponse>> GetAsync();
        Task<BlobResponse> GetAsync(Guid uid);
        Task<BaseResponse> UpdateAsync(UpdateBlobRequest request);
        Task<BaseResponse> DeleteAsync(Guid uid);
    }
}
