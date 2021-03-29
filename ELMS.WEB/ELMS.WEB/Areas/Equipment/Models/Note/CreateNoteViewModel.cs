using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models.Note
{
    public class CreateNoteViewModel
    {
        [Required]
        public Guid EquipmentUID { get; set; }

        public string OwnerUID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}