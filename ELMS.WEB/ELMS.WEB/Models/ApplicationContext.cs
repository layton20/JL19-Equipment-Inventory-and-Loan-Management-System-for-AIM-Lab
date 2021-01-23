﻿using ELMS.WEB.Entities.Equipment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ELMS.WEB.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<EquipmentEntity> Equipment { get; set; }
    }
}
