using ELMS.WEB.Enums.Admin;
using ELMS.WEB.Models.Base.Response;

namespace ELMS.WEB.Models.Admin.Response
{
    public class BlacklistResponse : BaseEntityResponse
    {
        public string Email { get; set; }
        public string Reason { get; set; }
        public BlacklistTypeEnum Type { get; set; }
        public bool Active { get; set; }
    }
}