using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models.Note
{
    public class DeleteNoteViewModel
    {
        [Required]
        public Guid UID { get; set; }

        [Required]
        public Guid EquipmentUID { get; set; }
    }
}