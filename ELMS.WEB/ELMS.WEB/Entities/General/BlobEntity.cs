using ELMS.WEB.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.General
{
    public class BlobEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }
    }
}