using System.Collections.Generic;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class ExpiredViewModel
    {
        public IList<EquipmentViewModel> ExpiredEquipment { get; set; } = new List<EquipmentViewModel>();
        public IList<EquipmentViewModel> WrittenOffEquipment { get; set; } = new List<EquipmentViewModel>();
    }
}
