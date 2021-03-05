using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Equipment.Request
{
    public class CreateEquipmentBlobRequest
    {
        [Required]
        public Guid EquipmentUID { get; set; }
        [Required]
        public Guid BlobUID { get; set; }
    }
}
