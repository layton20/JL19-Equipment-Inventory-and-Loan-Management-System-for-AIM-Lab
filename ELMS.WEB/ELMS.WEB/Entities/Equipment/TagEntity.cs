using ELMS.WEB.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Equipment
{
    public class TagEntity : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}