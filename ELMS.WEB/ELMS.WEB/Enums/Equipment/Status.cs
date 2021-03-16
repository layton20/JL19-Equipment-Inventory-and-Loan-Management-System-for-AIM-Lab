using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Equipment
{
    public enum Status : int
    {
        Unavailable = 0,

        Available = 1,

        [Display(Name = "Assigned to Pending Loan")]
        PendingLoan = 2,

        [Display(Name = "Assigned to Active Loan")]
        ActiveLoan = 3,

        [Display(Name = "Written Off")]
        WrittenOff = 4
    }
}