using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Equipment.Response
{
    public class EquipmentBlobResponse : BaseEntityResponse
    {
        public Guid EquipmentUID { get; set; }
        public Guid BlobUID { get; set; }
    }
}
