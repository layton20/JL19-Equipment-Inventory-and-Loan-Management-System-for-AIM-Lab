using ELMS.WEB.Enums.Admin;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Admin.Request
{
    public class CreateBlacklistRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Reason { get; set; } = "Reason not provided.";
        [Required]
        public BlacklistType Type { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
