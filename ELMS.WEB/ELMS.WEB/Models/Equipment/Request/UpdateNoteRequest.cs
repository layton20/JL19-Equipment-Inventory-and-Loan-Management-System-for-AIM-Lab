using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Equipment.Request
{
    public class UpdateNoteRequest
    {
        [Required]
        public Guid UID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}