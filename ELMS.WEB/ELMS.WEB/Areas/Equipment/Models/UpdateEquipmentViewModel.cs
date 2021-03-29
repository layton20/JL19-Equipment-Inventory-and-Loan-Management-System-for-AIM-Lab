using ELMS.WEB.Enums.Equipment;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class UpdateEquipmentViewModel
    {
        [Required]
        public Guid OwnerUID { get; set; }

        [Required]
        public Guid UID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]

        public string Description { get; set; } = "No description set.";

        [Required]
        [MaxLength(50)]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Purchase Price (£)")]
        [Range(0, 999999)]
        public double PurchasePrice { get; set; }

        [Display(Name = "Replacement Price (£)")]
        [Range(0, 999999)]
        public double ReplacementPrice { get; set; }

        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Display(Name = "Warranty Expiration Date")]
        public DateTime WarrantyExpirationDate { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Available;
    }
}