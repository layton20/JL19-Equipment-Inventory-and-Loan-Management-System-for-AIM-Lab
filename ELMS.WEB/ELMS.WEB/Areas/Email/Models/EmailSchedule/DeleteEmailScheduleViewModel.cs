using ELMS.WEB.CustomDataAnnotations;
using ELMS.WEB.Enums.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Areas.Email.Models.EmailSchedule
{
    public class DeleteEmailScheduleViewModel
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }

        public string RecipientEmail { get; set; }
        public EmailType EmailType { get; set; }
        public DateTime SendTimestamp { get; set; }
        public bool Sent { get; set; }
    }
}