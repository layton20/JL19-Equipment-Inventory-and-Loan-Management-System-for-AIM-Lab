using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.User
{
    public class IndexViewModel
    {
        public IList<IdentityUser> Users { get; set; }
    }
}