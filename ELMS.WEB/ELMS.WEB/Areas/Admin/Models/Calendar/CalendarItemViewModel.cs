using System;

namespace ELMS.WEB.Areas.Admin.Models.Calendar
{
    public class CalendarItemViewModel
    {
        public Guid ReferenceUID { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
    }
}