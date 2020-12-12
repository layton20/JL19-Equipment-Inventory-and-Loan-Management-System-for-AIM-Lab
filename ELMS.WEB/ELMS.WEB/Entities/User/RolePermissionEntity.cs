using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.User
{
    public class RolePermissionEntity : BaseEntity
    {
        [Required]
        public Guid RoleUID { get; set; }
        [Required]
        public Guid PermissionUID { get; set; }
    }
}
