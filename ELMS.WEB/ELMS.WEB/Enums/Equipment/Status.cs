namespace ELMS.WEB.Enums.Equipment
{
    public enum Status : int
    {
        Unavailable = 0,
        Available = 1,
        OnLoan = 2,
        Expired = 3,
        WrittenOff = 4,
        NonLoanable = 5
    }
}