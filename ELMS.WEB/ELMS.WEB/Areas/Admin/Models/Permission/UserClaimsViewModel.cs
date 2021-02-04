using ELMS.WEB.Areas.Admin.Data;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Permission
{
    public class UserClaimsViewModel
    {
        public string UserID { get; set; }
        public IList<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
