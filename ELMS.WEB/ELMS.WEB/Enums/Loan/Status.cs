using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Loan
{
    public enum Status : int
    {
        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Out On-Loan")]
        OnLoan = 1,

        [Display(Name = "Completed")]
        Complete = 2,

        [Display(Name = "Manually Completed")]
        ManualComplete = 3,
    }
}