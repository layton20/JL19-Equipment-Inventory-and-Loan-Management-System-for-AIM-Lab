using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Admin.Models.Permission
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}