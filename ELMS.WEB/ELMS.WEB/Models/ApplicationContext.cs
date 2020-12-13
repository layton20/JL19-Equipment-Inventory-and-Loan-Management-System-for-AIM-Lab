using ELMS.WEB.Entities.Equipment;
using Microsoft.EntityFrameworkCore;

namespace ELMS.WEB.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<EquipmentEntity> Equipment { get; set; }
    }
}
