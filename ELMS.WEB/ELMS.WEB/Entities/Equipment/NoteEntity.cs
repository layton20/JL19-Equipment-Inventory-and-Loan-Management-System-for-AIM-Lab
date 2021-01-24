using ELMS.WEB.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Entities.Equipment
{
    public class NoteEntity : BaseEntity
    {
        [Required]
        [ForeignKey("Owner")]
        public string OwnerUID { get; set; }
        [Required]
        [ForeignKey("Equipment")]
        public Guid EquipmentUID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public IdentityUser Owner { get; set; }
        public EquipmentEntity Equipment { get; set; }
    }
}