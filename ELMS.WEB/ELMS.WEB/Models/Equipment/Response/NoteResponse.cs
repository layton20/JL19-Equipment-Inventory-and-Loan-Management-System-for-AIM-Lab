using ELMS.WEB.Models.Base.Response;
using Microsoft.AspNetCore.Identity;
using System;

namespace ELMS.WEB.Models.Equipment.Response
{
    public class NoteResponse : BaseEntityResponse
    {
        public Guid EquipmentUID { get; set; }
        public Guid OwnerUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IdentityUser Owner { get; set; }
    }
}