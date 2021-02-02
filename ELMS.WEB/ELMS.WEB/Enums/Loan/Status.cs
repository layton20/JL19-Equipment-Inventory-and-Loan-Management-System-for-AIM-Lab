using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Loan
{
    public enum Status : int
    {
        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Inactive Loan")]
        InactiveLoan = 1,
        
        [Display(Name = "Active Loan")]
        ActiveLoan = 2,

        [Display(Name = "Completed")]
        Complete = 3,

        [Display(Name = "Manually Completed")]
        ManualComplete = 4,
    }
}