using ELMS.WEB.Enums.Equipment;
using System;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class EquipmentViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; } = "No description set.";
        public string SerialNumber { get; set; }

        public double PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpirationDate { get; set; }

        public Status Status { get; set; } = Status.Unavailable;
    }
}
