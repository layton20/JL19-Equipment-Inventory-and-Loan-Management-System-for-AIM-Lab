using ELMS.WEB.CustomDataAnnotations;
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
        [MaxLength(1000)]

        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; }
        [Range(0, 999999)]

        public double PurchasePrice { get; set; }
        [Range(0, 999999)]
        public double ReplacementPrice { get; set; }
        [FutureOrTodayDate]

        public DateTime PurchaseDate { get; set; }
        [FutureDate]

        [Required]
        public DateTime WarrantyExpirationDate { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Available;
    }
}