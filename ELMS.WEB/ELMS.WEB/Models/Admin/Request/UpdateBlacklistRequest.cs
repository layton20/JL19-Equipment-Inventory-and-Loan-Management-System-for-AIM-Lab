using ELMS.WEB.Enums.Admin;
using ELMS.WEB.Models.Base.Request;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Admin.Request
{
    public class UpdateBlacklistRequest : UIDRequest
    {
        [Required]
        public string Reason { get; set; } = "Reason not provided.";

        [Required]
        public bool Active { get; set; }

        [Required]
        public BlacklistTypeEnum Type { get; set; }
    }
}