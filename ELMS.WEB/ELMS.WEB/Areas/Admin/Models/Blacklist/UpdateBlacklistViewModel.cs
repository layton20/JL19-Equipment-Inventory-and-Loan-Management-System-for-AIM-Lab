using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Admin;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Blacklist
{
    public class UpdateBlacklistViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public BlacklistType Type { get; set; }
        [Required]
        public string Reason { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
