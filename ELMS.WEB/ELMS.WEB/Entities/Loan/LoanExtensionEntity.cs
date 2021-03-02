using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Entities.Loan
{
    public class LoanExtensionEntity : BaseEntity
    {
        [ForeignKey("Loan")]
        [NonDefaultGuid]
        [Required]
        public Guid LoanUID { get; set; }

        [Required]
        [EmailAddress]
        public string ExtenderEmail { get; set; }

        [Required]
        public DateTime PreviousExpiryDate { get; set; }

        [Required]
        [FutureOrTodayDate]
        public DateTime NewExpiryDate { get; set; }

        public LoanEntity Loan { get; set; }
    }
}