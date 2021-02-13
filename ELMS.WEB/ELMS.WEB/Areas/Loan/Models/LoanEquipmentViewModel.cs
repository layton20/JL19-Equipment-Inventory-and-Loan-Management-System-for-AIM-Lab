using ELMS.WEB.Areas.Equipment.Models;
using System;

namespace ELMS.WEB.Areas.Loan.Models
{
    public class LoanEquipmentViewModel
    {
        public Guid UID { get; set; }
        public EquipmentViewModel Equipment { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}