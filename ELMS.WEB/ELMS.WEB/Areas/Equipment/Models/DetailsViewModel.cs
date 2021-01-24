using ELMS.WEB.Areas.Equipment.Models.Note;
using System.Collections.Generic;

namespace ELMS.WEB.Areas.Equipment.Models
{
    public class DetailsViewModel
    {
        public EquipmentViewModel Equipment { get; set; }
        public IList<NoteViewModel> Notes { get; set; }
    }
}
