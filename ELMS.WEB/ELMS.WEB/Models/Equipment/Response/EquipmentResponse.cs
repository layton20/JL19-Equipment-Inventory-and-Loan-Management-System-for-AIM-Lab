using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Equipment.Response
{
    public class EquipmentResponse : BaseEntityResponse
    {
        public Guid OwnerUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public double PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpirationDate { get; set; }
        public Status Status { get; set; }
    }
}
