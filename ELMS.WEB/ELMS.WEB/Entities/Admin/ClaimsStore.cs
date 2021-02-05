using System.Collections.Generic;
using System.Security.Claims;

namespace ELMS.WEB.Entities.Admin
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            // Loans
            new Claim("Create Loan", "Create Loan"),
            new Claim("Edit Loan", "Edit Loan"),
            new Claim("Delete Loan", "Delete Loan"),
            new Claim("View Loan", "View Loan"),

            // Equipment
            new Claim("Create Equipment", "Create Equipment"),
            new Claim("Edit Equipment", "Edit Equipment"),
            new Claim("Delete Equipment", "Delete Equipment"),
            new Claim("View Equipment", "View Equipment"),

            // Notes
            new Claim("Create Note", "Create Note"),
            new Claim("Edit Note", "Edit Note"),
            new Claim("Delete Note", "Delete Note"),
            new Claim("View Note", "View Note"),

            // EmailSchedules
            new Claim("Create EmailSchedule", "Create EmailSchedule"),
            new Claim("Edit EmailSchedule", "Edit EmailSchedule"),
            new Claim("Delete EmailSchedule", "Delete EmailSchedule"),
            new Claim("View EmailSchedule", "View EmailSchedule"),

            // Users
            new Claim("Create User", "Create User"),
            new Claim("Edit User", "Edit User"),
            new Claim("Delete User", "Delete User"),
            new Claim("View User", "View User"),
        };
    }
}
