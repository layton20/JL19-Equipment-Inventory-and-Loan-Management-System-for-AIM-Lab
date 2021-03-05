using ELMS.WEB.Entities.Base;
using ELMS.WEB.Entities.General;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Entities.Equipment
{
    public class EquipmentBlobEntity : BaseEntity
    {
        [Required]
        public Guid EquipmentUID { get; set; }
        [Required]
        public Guid BlobUID { get; set; }
        [ForeignKey("EquipmentUID")]
        public EquipmentEntity Equipment { get; set; }
        [ForeignKey("BlobUID")]
        public BlobEntity Blob { get; set; }
    }
}
