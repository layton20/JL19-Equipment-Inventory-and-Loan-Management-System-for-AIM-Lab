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
        public Guid LoanUID { get; set; }

        [Required]
        public Guid EquipmentUID { get; set; }

        [ForeignKey("LoanUID")]
        public LoanEntity Loan { get; set; }

        [ForeignKey("EquipmentUID")]
        public EquipmentEntity Equipment { get; set; }
    }
}