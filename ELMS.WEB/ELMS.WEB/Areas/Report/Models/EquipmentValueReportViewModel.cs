using System.Collections.Generic;

namespace ELMS.WEB.Areas.Report.Models
{
    public class EquipmentValueReportViewModel
    {
        public IList<EquipmentValueReportItemViewModel> ReportItems { get; set; } = new List<EquipmentValueReportItemViewModel>();
        public EquipmentValueReportFilterViewModel Filter { get; set; } = new EquipmentValueReportFilterViewModel();
    }
}