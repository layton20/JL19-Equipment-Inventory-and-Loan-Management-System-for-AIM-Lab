using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Equipment
{
    public class EquipmentTagEntity : BaseEntity
    {
        [Required]
        public Guid EquipmentUID { get; set; }
        [Required]
        public Guid TagUID { get; set; }
    }
}
