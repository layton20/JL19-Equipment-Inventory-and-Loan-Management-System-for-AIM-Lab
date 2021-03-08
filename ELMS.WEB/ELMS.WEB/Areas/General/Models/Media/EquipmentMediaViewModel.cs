using System;

namespace ELMS.WEB.Areas.General.Models.Media
{
    public class EquipmentMediaViewModel
    {
        public Guid UID { get; set; }
        public Guid EquipmentUID { get; set; }
        public Guid BlobUID { get; set; }
        public MediaViewModel Blob { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime AmendedTimestamp { get; set; }
    }
}