using System.Collections.Generic;
using System.Security.Claims;

namespace ELMS.WEB.Entities.Admin
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            // Loans
            new Claim("Loan", "Create Loan"),

            // Equipment
            new Claim("Equipment", "Create Equipment"),
            new Claim("Equipment", "Edit Equipment"),
            new Claim("Equipment", "Delete Equipment"),
            new Claim("Equipment", "View Equipment"),
        };
    }
}
