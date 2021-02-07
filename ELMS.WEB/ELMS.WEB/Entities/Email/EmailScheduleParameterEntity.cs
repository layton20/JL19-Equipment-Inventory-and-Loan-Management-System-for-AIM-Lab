using ELMS.WEB.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.WEB.Entities.Email
{
    public class EmailScheduleParameterEntity : BaseEntity
    {
        [Required]
        [ForeignKey("EmailSchedule")]
        public Guid EmailScheduleUID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        public EmailScheduleEntity EmailSchedule { get; set; }
    }
}
