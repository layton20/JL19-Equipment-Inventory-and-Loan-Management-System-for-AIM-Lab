using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Equipment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class FilterEquipmentViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Purchase Price From")]
        [Range(0, double.MaxValue)]
        public double PurchasePriceFrom { get; set; } = 0;
        [Display(Name = "Purchase Price To")]
        [Range(0, double.MaxValue)]
        public double PurchasePriceTo { get; set; } = 10000;
        [Display(Name = "Purchase Date From")]
        [PastOrTodayDate]
        public DateTime PurchaseDateFrom { get; set; } = DateTime.MinValue;
        [PastOrTodayDate]
        [Display(Name = "Purchase Date To")]
        public DateTime PurchaseDateTo { get; set; } = DateTime.Today.AddDays(1);
        public IList<Status> Statuses { get; set; }
        [Display(Name = "Warranty Expiration Date From")]
        public DateTime WarrantyExpirationDateFrom { get; set; } = DateTime.MinValue;
        [Display(Name = "Warranty Expiration Date To")]
        public DateTime WarrantyExpirationDateTo { get; set; } = DateTime.MaxValue;
    }
}
