using ELMS.WEB.Areas.Admin.Models.Permission;
using ELMS.WEB.Areas.Loan.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.User
{
    public class DetailsViewModel
    {
        public IdentityUser User { get; set; }
        public IList<LoanViewModel> Loans { get; set; } = new List<LoanViewModel>();
        public IList<string> Roles { get; set; } = new List<string>();
        public UserClaimsViewModel UserClaims { get; set; } = new UserClaimsViewModel();
    }
}