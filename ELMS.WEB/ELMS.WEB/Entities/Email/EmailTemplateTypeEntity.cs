using ELMS.WEB.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Email
{
    public class EmailTemplateTypeEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Code { get; set; }
    }
}
