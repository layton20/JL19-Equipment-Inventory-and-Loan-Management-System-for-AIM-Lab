using ELMS.WEB.CustomDataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Base.Request
{
    public class UIDRequest
    {
        [Required]
        [NonDefaultGuid]
        public Guid UID { get; set; }
    }
}