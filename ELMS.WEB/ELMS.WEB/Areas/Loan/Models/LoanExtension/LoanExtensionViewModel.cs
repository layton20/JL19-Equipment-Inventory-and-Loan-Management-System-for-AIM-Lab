using System;

namespace ELMS.WEB.Areas.Loan.Models.LoanExtension
{
    public class LoanExtensionViewModel
    {
        public Guid UID { get; set; }
        public Guid LoanUID { get; set; }
        public string ExtenderEmail { get; set; }
        public DateTime PreviousExpiryDate { get; set; }
        public DateTime NewExpiryDate { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}