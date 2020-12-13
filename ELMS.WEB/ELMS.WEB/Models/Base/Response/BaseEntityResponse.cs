using System;

namespace ELMS.WEB.Models.Base.Response
{
    public class BaseEntityResponse : BaseResponse
    {
        public Guid UID { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}