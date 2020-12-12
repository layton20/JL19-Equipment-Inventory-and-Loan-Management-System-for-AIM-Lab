using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Equipment
{
    public class NoteEntity : BaseEntity
    {
        [Required]
        public int EquipmentID { get; set; }
        [Required]
        public int UserID { get; set; }
        public string Description { get; set; }
        public EquipmentEntity Equipment { get; set; }
    }
}
