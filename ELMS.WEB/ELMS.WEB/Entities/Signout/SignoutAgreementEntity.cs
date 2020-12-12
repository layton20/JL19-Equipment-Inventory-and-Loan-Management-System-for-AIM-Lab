using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Signout;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Signout
{
    public class SignoutAgreementEntity : BaseEntity
    {
        public Guid LoanerUID { get; set; }
        [Required]
        public Guid LoaneeUID { get; set; }
        [Required]
        public Status Status { get; set; } = Status.Pending;
        [Required]
        public DateTime SignoutDate { get; set; }
    }
}
