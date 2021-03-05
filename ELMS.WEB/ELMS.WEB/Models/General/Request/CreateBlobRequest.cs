using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.General.Request
{
    public class CreateBlobRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
