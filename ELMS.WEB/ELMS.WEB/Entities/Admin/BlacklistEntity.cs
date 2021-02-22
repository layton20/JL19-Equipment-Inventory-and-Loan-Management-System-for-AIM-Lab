using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Admin;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Admin
{
    public class BlacklistEntity : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Reason { get; set; } = "Reason not provided.";

        [Required]
        public bool Active { get; set; }

        [Required]
        public BlacklistTypeEnum Type { get; set; }
    }
}