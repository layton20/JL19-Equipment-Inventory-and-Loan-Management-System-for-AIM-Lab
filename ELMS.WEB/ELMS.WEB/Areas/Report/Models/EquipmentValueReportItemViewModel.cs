using ELMS.WEB.Enums.Equipment;
using System;

namespace ELMS.WEB.Areas.Report.Models
{
    public class EquipmentValueReportItemViewModel
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public double PurchasePrice { get; set; }
        public double ReplacementPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpirationDate { get; set; }
        public Status Status { get; set; }
    }
}