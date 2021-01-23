using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Equipment;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Equipment
{
    public class EquipmentEntity : BaseEntity
    {
        [Required]
        public Guid OwnerUID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; } = "No description set.";

        [Required]
        public string SerialNumber { get; set; }

        public double PurchasePrice { get; set; }

        public DateTime PurchaseDate { get; set; }

        [Required]
        public DateTime WarrantyExpirationDate { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Available;
    }
}