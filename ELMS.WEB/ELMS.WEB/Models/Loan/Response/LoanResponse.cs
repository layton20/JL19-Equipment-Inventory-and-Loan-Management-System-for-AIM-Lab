using ELMS.WEB.Enums.Loan;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Collections.Generic;

namespace ELMS.WEB.Models.Loan.Response
{
    public class LoanResponse : BaseResponse
    {
        public Guid UID { get; set; }
        public string LoanerEmail { get; set; }
        public string LoaneeEmail { get; set; }
        public DateTime FromTimestamp { get; set; } = DateTime.Now;
        public DateTime ExpiryTimestamp { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public bool AcceptedTermsAndConditions { get; set; } = false;
        public DateTime CompletedTimestamp { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
        public IList<EquipmentResponse> EquipmentList { get; set; } = new List<EquipmentResponse>();
        public IList<LoanExtensionResponse> Extensions { get; set; } = new List<LoanExtensionResponse>();
    }
}