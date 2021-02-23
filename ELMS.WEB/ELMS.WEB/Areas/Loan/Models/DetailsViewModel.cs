using ELMS.WEB.Areas.Loan.Models.LoanExtension;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class DetailsViewModel
    {
        public LoanViewModel Loan { get; set; }
        public IList<LoanExtensionViewModel> Extensions { get; set; } = new List<LoanExtensionViewModel>();
    }
}
