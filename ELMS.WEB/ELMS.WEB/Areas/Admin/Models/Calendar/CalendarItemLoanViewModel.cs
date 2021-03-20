using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Calendar
{
    public class CalendarItemLoanViewModel : CalendarItemViewModel
    {
        public string LoaneeEmail { get; set; }
        public IList<string> EquipmentNames { get; set; }
    }
}