using ELMS.WEB.Enums.Admin;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Blacklist
{
    public class CreateViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        [Display(Name = "Blacklist Type")]
        public BlacklistTypeEnum Type { get; set; }

        [Required]
        public bool Active { get; set; } = true;
    }
}