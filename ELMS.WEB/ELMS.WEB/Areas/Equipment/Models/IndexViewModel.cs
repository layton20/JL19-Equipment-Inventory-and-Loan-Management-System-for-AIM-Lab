using System.Collections.Generic;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class IndexViewModel
    {
        public IList<EquipmentViewModel> Equipment { get; set; } = new List<EquipmentViewModel>();
        public FilterEquipmentViewModel Filter { get; set; } = new FilterEquipmentViewModel();
        public int AvailableEquipmentCount { get; set; }
        public int OnLoanEquipmentCount { get; set; }
        public int WarrantyExpiredEquipmentCount { get; set; }
    }
}