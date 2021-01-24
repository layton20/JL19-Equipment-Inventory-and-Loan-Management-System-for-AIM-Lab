using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models.Note
{
    public class NoteViewModel
    {
        [Required]
        public Guid UID { get; set; }

        public Guid EquipmentUID { get; set; }
        public string OwnerUID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedTimestamp { get; set; }
    }
}