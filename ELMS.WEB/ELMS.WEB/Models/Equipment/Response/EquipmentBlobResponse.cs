using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.General.Response;
using System;

namespace ELMS.WEB.Models.Equipment.Response
{
    public class EquipmentBlobResponse : BaseEntityResponse
    {
        public Guid EquipmentUID { get; set; }
        public Guid BlobUID { get; set; }
        public BlobResponse Blob { get; set; }
    }
}