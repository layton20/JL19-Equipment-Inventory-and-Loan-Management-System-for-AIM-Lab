using ELMS.WEB.Enums.Equipment;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Equipment.Request
{
    public class UpdateEquipmentRequest
    {
        [Required]
        public Guid UID { get; set; }

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