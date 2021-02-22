using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Blacklist
{
    public class DetailsModalViewModel
    {
        public IList<BlacklistViewModel> Blacklists { get; set; } = new List<BlacklistViewModel>();
    }
}
