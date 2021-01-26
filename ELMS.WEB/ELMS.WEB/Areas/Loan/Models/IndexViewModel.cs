using System.Collections.Generic;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class IndexViewModel
    {
        public IList<LoanViewModel> Loans { get; set; } = new List<LoanViewModel>();
    }
}