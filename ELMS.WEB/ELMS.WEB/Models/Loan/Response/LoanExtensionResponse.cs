using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Loan.Response
{
    public class LoanExtensionResponse : BaseEntityResponse
    {
        public Guid LoanUID { get; set; }
        public string ExtenderEmail { get; set; }
        public DateTime PreviousExpiryDate { get; set; }
        public DateTime NewExpiryDate { get; set; }
    }
}
