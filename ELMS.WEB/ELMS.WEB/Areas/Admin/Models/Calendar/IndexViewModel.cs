using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Calendar
{
    public class IndexViewModel
    {
        public IList<CalendarItemLoanViewModel> Loans { get; set; } = new List<CalendarItemLoanViewModel>();
        public IList<CalendarItemEquipmentViewModel> Equipment { get; set; } = new List<CalendarItemEquipmentViewModel>();
    }
}
