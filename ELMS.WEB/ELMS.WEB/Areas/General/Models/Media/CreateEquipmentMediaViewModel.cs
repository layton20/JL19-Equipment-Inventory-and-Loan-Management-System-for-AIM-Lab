using ELMS.WEB.CustomDataAnnotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.General.Models.Media
{
    public class CreateEquipmentMediaViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid EquipmentUID { get; set; }

        [Required]
        public IList<IFormFile> MediaFiles { get; set; }
    }
}