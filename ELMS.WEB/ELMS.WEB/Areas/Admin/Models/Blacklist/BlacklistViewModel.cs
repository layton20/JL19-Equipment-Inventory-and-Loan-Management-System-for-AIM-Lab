using ELMS.WEB.Enums.Admin;
using System;

namespace ELMS.WEB.Areas.Admin.Models.Blacklist
{
    public class BlacklistViewModel
    {
        public Guid UID { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
        public BlacklistType Type { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}