using ELMS.WEB.Entities.Email;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Entities.Loan;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ELMS.WEB.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<EquipmentEntity> Equipment { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<LoanEntity> Loans { get; set; }
        public DbSet<LoanEquipmentEntity> LoanEquipmentList { get; set; }
        public DbSet<EmailTemplateEntity> EmailTemplates { get; set; }
        public DbSet<EmailScheduleEntity> EmailSchedules { get; set; }
    }
}