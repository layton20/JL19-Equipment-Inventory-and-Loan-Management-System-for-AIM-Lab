using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Role
{
    public class UpdateRoleViewModal
    {
        [Required]
        public String UID { get; set; }
        [Required]
        public String Name { get; set; }
    }
}
