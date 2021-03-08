using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.General.Models.Media
{
    public class DeleteEquipmentMediaViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }
    }
}