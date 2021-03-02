using ELMS.WEB.Enums.Equipment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Report.Models
{
    public class EquipmentValueReportFilterViewModel
    {
        [Display(Name = "Equipment Name")]
        public string Name { get; set; }

        [Display(Name = "Equipment Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Purchase Price From (£)")]
        public double PurchasePriceFrom { get; set; } = 0;

        [Display(Name = "Purchase Price To (£)")]
        public double PurchasePriceTo { get; set; } = 999999;

        [Display(Name = "Replacement Price From (£)")]
        public double ReplacementPriceFrom { get; set; } = 0;

        [Display(Name = "Replacement Price To (£)")]
        public double ReplacementPriceTo { get; set; } = 999999;

        [Display(Name = "Purchase Date From")]
        public DateTime PurchaseDateFrom { get; set; } = DateTime.MinValue;

        [Display(Name = "Purchase Date To")]
        public DateTime PurchaseDateTo { get; set; } = DateTime.Today;

        [Display(Name = "Warranty Expiration Date From")]
        public DateTime WarrantyExpirationDateFrom { get; set; } = DateTime.MinValue;

        [Display(Name = "Warranty Expiration Date To")]
        public DateTime WarrantyExpirationDateTo { get; set; } = DateTime.Today;

        [Display(Name = "Statuses")]
        public IList<Status> Statuses { get; set; }
    }
}