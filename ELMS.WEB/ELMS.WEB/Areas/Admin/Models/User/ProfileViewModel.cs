using ELMS.WEB.Areas.Loan.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.User
{
    public class ProfileViewModel
    {
        public IdentityUser User { get; set; }
        public IList<LoanViewModel> UserIsLoanerLoans { get; set; } = new List<LoanViewModel>();
        public IList<LoanViewModel> UserIsLoaneeLoans { get; set; } = new List<LoanViewModel>();
    }
}
