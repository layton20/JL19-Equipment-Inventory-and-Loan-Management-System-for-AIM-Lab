using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Models.General.Request
{
    public class UpdateBlobRequest
    {
        [Required]
        public Guid UID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}