using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Equipment.Request
{
    public class CreateNoteRequest
    {
        [Required]
        public string OwnerUID { get; set; }

        [Required]
        public Guid EquipmentUID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}