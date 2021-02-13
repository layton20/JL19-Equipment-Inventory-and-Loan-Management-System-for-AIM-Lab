using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Role
{
    public class CreateRoleViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}