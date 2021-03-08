namespace ELMS.WEB.Areas.Admin.Models.Job
{
    public class JobRunningStatuses
    {
        public bool IsEquipmentJobRunning { get; set; } = false;
        public bool IsLoanJobRunning { get; set; } = false;
        public bool IsEmailJobRunning { get; set; } = false;
    }
}
