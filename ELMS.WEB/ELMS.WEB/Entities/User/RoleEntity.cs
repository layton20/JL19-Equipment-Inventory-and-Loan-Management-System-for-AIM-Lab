using ELMS.WEB.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.User
{
    public class RoleEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
