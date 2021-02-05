using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Admin
{
    public class RoleEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
