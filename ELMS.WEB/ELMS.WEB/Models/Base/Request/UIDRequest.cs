using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.Base.Request
{
    public class UIDRequest
    {
        [Required]
        public Guid UID { get; set; }
    }
}
