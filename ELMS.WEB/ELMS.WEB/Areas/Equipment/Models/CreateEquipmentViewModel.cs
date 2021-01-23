using ELMS.WEB.Enums.Equipment;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class CreateEquipmentViewModel
    {   
        [Required]
        public string Name { get; set; }

        public string Description { get; set; } = "No description set.";
        [Required]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Required]
        [Display(Name = "Purchase Price (£)")]

        public double PurchasePrice { get; set; }
        [Required]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }
        [Required]
        [Display(Name = "Warranty Expiration Date")]
        public DateTime WarrantyExpirationDate { get; set; }
        [Required]

        public InitialStatus Status { get; set; } = InitialStatus.Available;
        [Required]
        [Range(0, 500)]
        public int Quantity { get; set; } = 1;
    }
}
