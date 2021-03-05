using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Areas.General.Models.Media;
using System;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class DetailsViewModel
    {
        public EquipmentViewModel Equipment { get; set; }
        public IList<NoteViewModel> Notes { get; set; }
        public CreateEquipmentMediaViewModel UploadMedia { get; set; }
        public IList<EquipmentMediaViewModel> EquipmentMedia { get; set; } = new List<EquipmentMediaViewModel>();
    }
}