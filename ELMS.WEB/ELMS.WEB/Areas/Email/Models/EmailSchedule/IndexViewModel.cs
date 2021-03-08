using System.Collections.Generic;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class IndexViewModel
    {
        public IList<EmailScheduleViewModel> EmailSchedules { get; set; } = new List<EmailScheduleViewModel>();
        public EmailScheduleFilterViewModel Filter { get; set; }
    }
}