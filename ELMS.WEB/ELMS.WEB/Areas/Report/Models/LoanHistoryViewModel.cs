using System.Collections.Generic;

namespace ELMS.WEB.Areas.Report.Models
{
    public class LoanHistoryViewModel
    {
        public IList<LoanHistoryItemViewModel> Loans { get; set; } = new List<LoanHistoryItemViewModel>();
        public LoanHistoryReportFilterViewModel Filter { get; set; } = new LoanHistoryReportFilterViewModel();
    }
}
