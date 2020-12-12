using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Signout
{
    public class SignoutAgreementEquipmentEntity : BaseEntity
    {
        [Required]
        public Guid SignoutAgreementUID { get; set; }
        [Required]
        public Guid EquipmentUID { get; set; }
    }
}
