using ELMS.WEB.Areas.Equipment.Models;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class IndexViewModel
    {
        public IList<LoanViewModel> Loans { get; set; } = new List<LoanViewModel>();
        public LoanFilterViewModel Filter { get; set; } = new LoanFilterViewModel();
    }
}