using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Equipment
{
    public class EquipmentBlobEntity : BaseEntity
    {
        [Required]
        public Guid EquipmentUID { get; set; }
        [Required]
        public Guid BlobUID { get; set; }
    }
}
