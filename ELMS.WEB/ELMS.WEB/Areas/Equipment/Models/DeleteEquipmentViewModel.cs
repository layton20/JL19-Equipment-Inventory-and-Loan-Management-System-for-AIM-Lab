using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class DeleteEquipmentViewModel
    {
        [Required]
        public Guid UID { get; set; }
    }
}