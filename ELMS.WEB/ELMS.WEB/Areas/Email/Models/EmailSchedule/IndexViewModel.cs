using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class IndexViewModel
    {
        public IList<EmailScheduleViewModel> EmailSchedules { get; set; }
    }
}
