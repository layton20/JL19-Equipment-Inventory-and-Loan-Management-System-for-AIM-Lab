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

            // Configurations
            new Claim("Create Configuration", "Create Configuration"),
            new Claim("Edit Configuration", "Edit Configuration"),
            new Claim("Delete Configuration", "Delete Configuration"),
            new Claim("View Configuration", "View Configuration"),

            // LoanExtensions
            new Claim("Create LoanExtension", "Create LoanExtension"),
            new Claim("Edit LoanExtension", "Edit LoanExtension"),
            new Claim("Delete LoanExtension", "Delete LoanExtension"),
            new Claim("View LoanExtension", "View LoanExtension"),

            // Reports
            new Claim("View Report", "View Report"),
            new Claim("Filter Report", "Filter Report"),

            // EmailTemplates
            new Claim("Create EmailTemplate", "Create EmailTemplate"),
            new Claim("Edit EmailTemplate", "Edit EmailTemplate"),
            new Claim("Delete EmailTemplate", "Delete EmailTemplate"),
            new Claim("View EmailTemplate", "View EmailTemplate"),

            // EmailSchedules
            new Claim("Create EmailSchedule", "Create EmailSchedule"),
            new Claim("Edit EmailSchedule", "Edit EmailSchedule"),
            new Claim("Delete EmailSchedule", "Delete EmailSchedule"),
            new Claim("View EmailSchedule", "View EmailSchedule"),
            new Claim("Send EmailSchedule", "View EmailSchedule"),

            // EmailLogs
            new Claim("View EmailLog", "View EmailLog"),

            // JobScheduler
            new Claim("Run JobScheduler", "Run JobScheduler"),

            // Calendar
            new Claim("View Calendar", "View Calendar"),

            // Blacklists
            new Claim("Create Blacklist", "Create Blacklist"),
            new Claim("Edit Blacklist", "Edit Blacklist"),
            new Claim("Delete Blacklist", "Delete Blacklist"),
            new Claim("View Blacklist", "View Blacklist"),

            // Users
            new Claim("Create User", "Create User"),
            new Claim("Edit User", "Edit User"),
            new Claim("Delete User", "Delete User"),
            new Claim("View User", "View User"),
        };
    }
}