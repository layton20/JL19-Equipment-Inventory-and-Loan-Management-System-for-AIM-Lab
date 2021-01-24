using ELMS.WEB.Models.Base.Response;
using System;

namespace ELMS.WEB.Models.Equipment.Response
{
    public class NoteResponse : BaseEntityResponse
    {
        public Guid EquipmentUID { get; set; }
        public Guid OwnerUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
