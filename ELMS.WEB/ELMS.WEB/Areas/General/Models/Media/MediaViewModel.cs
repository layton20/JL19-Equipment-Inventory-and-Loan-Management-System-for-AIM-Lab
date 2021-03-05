using System;

namespace ELMS.WEB.Areas.General.Models.Media
{
    public class MediaViewModel
    {
        public Guid UID { get; set; }
        public string Path { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}
