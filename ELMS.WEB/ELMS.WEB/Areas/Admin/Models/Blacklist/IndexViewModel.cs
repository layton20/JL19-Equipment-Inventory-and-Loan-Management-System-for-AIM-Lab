using System.Collections.Generic;

namespace ELMS.WEB.Areas.Admin.Models.Blacklist
{
    public class IndexViewModel
    {
        public IList<BlacklistViewModel> Blacklists { get; set; }
    }
}