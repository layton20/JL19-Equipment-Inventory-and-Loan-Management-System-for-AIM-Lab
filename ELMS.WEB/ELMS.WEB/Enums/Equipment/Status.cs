using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Equipment
{
    public enum Status : int
    {
        Unavailable = 0,
        Available = 1,

        [Display(Name = "On Loan")]
        OnLoan = 2,

        Expired = 3,

        [Display(Name = "Written Off")]
        WrittenOff = 4,

        [Display(Name = "Not Loanable")]
        NonLoanable = 5
    }
}