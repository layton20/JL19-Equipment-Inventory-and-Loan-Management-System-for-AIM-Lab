using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Loan.Response
{
    public class LoanEquipmentResponse : BaseEntityResponse
    {
        public Guid EquipmentUID { get; set; }
        public Guid LoanUID { get; set; }
    }
}
