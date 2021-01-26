using ELMS.WEB.Entities.Base;
using ELMS.WEB.Entities.Equipment;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Entities.Loan
{
    public class LoanEquipmentEntity : BaseEntity
    {
        [Required]
        [ForeignKey("Loan")]
        public Guid LoanUID { get; set; }

        [Required]
        [ForeignKey("Equipment")]
        public Guid EquipmentUID { get; set; }

        public LoanEntity Loan { get; set; }
        public EquipmentEntity Equipment { get; set; }
    }
}