using ELMS.WEB.Entities.Base;
using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Entities.Email
{
    public class EmailEntity : BaseEntity
    {
        [Required]
        public Guid EmailTemplateUID { get; set; }
        public Guid SenderUID { get; set; }
        [Required]
        public Guid ReceiverUID { get; set; }
        [Required]
        public Status Status { get; set; } = Status.Scheduled;
    }
}
