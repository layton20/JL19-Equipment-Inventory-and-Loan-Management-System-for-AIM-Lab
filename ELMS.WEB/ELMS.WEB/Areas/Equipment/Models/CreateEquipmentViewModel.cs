using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Equipment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class CreateEquipmentViewModel
    {
        public string OwnerUID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Required]
        [Display(Name = "Purchase Price (£)")]
        public double PurchasePrice { get; set; }

        [Required]
        [Display(Name = "Replacement Price (£)")]
        public double ReplacementPrice { get; set; }

        [Required]
        [PastOrTodayDate]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        [FutureDate]
        [Display(Name = "Warranty Expiration Date")]
        public DateTime WarrantyExpirationDate { get; set; } = DateTime.Now;

        [Required]
        public InitialStatus Status { get; set; } = InitialStatus.Available;

        [Required]
        [Range(0, 500)]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Upload File(s)")]
        public IList<IFormFile> MediaFiles { get; set; }
    }
}