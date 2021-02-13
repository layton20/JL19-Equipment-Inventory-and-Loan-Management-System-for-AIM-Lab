using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Email
{
    public enum EmailScheduleStatus : int
    {
        Scheduled = 0,
        Sent = 1,

        [Display(Name = "Manually Sent")]
        ManuallySent = 2,
    }
}