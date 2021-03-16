using System.Collections.Generic;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class IndexViewModel
    {
        public IList<LoanViewModel> Loans { get; set; } = new List<LoanViewModel>();
        public LoanFilterViewModel Filter { get; set; } = new LoanFilterViewModel();
        public int ActiveLoansCount { get; set; }
        public int PendingLoansCount { get; set; }
        public int OverdueLoansCount { get; set; }
    }
}