using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Equipment
{
    public class EquipmentNoteEntity : BaseEntity
    {
        [Required]
        public Guid EquipmentUID { get; set; }

        [Required]
        public Guid NoteUID { get; set; }
    }
}