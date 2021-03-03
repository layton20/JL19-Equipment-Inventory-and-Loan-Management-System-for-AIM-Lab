using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.Loan.Models;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Dashboard
{
    public class IndexViewModel
    {
        public IList<LoanViewModel> Loans { get; set; } = new List<LoanViewModel>();
        public IList<EquipmentViewModel> EquipmentList { get; set; } = new List<EquipmentViewModel>();
    }
}
