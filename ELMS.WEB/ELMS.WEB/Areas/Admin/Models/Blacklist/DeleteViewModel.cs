using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Admin;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Blacklist
{
    public class DeleteViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }

        public string Email { get; set; }
        public string Reason { get; set; }
        public BlacklistType Type { get; set; }
        public bool Active { get; set; }
    }
}