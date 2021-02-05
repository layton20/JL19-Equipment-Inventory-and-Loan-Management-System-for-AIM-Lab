using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Role
{
    public class IndexViewModel
    {
        public IList<IdentityRole> Roles { get; set; }
    }
}
