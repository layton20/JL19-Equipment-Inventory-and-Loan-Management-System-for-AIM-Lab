using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Base
{
    public abstract class BaseEntity
    {
        [Required]
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime CreatedTimestamp { get; set; } = DateTime.Now;

        [Required]
        public DateTime AmendedTimestamp { get; set; } = DateTime.Now;

        [Required]
        public Guid UID { get; set; } = Guid.NewGuid();
    }
}