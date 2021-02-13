using System.Collections.Generic;
using System.Security.Claims;

namespace ELMS.WEB.Areas.Admin.Models.Permission
{
    public class IndexViewModel
    {
        public IList<Claim> Claims { get; set; } = new List<Claim>();
    }
}