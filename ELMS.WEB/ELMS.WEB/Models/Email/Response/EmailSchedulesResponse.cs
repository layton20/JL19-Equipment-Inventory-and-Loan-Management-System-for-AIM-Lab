using ELMS.WEB.Models.Base.Response;
using System.Collections.Generic;

namespace ELMS.WEB.Models.Email.Response
{
    public class EmailSchedulesResponse : BaseResponse
    {
        public IList<EmailScheduleResponse> Responses { get; set; }
    }
}
