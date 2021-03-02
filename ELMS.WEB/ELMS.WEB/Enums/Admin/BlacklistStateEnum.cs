using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Admin
{
    public enum BlacklistStateEnum : int
    {
        [Display(Name = "No Blacklist History")]
        None = 0,

        [Display(Name = "Previously Blacklisted")]
        HistoricBlacklist = 1,

        [Display(Name = "Currently Blacklisted")]
        ActiveBlacklist = 2
    }
}