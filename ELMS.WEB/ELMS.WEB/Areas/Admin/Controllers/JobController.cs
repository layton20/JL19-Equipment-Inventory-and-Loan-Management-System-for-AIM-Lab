using ELMS.WEB.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class JobController : Controller
    {
        private readonly IScheduler __Scheduler;

        public JobController(IScheduler scheduler)
        {
            __Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        public IActionResult Index(string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }
            else if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StartEquipmentJobAsync()
        {
            IJobDetail _Job = JobBuilder.Create<EquipmentJob>()
                .WithIdentity("equipment", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_Job.Key))
            {
                return RedirectToAction("Index", "Job", new { Area = "Admin", errorMessage = $"Already triggered Equipment job scheduler." });
            }

            ITrigger _Trigger = TriggerBuilder.Create()
                .WithIdentity("equipmentTrigger", "elmsJobs")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
                .Build();

            await __Scheduler.ScheduleJob(_Job, _Trigger);

            return RedirectToAction("Index", "Job", new { Area = "Admin", successMessage = $"Successfully triggered Equipment job scheduler." });
        }

        [HttpGet]
        public async Task<IActionResult> StartLoanJobAsync()
        {
            IJobDetail _Job = JobBuilder.Create<LoanJob>()
                .WithIdentity("loan", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_Job.Key))
            {
                return RedirectToAction("Index", "Job", new { Area = "Admin", errorMessage = $"Already triggered Loan job scheduler." });
            }

            ITrigger _Trigger = TriggerBuilder.Create()
                .WithIdentity("loanTrigger", "elmsJobs")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
                .Build();

            await __Scheduler.ScheduleJob(_Job, _Trigger);

            return RedirectToAction("Index", "Job", new { Area = "Admin", successMessage = $"Successfully triggered Loan job scheduler." });
        }

        [HttpGet]
        public async Task<IActionResult> StartEmailJobAsync()
        {
            IJobDetail _Job = JobBuilder.Create<EmailJob>()
                .WithIdentity("email", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_Job.Key))
            {
                return RedirectToAction("Index", "Job", new { Area = "Admin", errorMessage = $"Already triggered Email job scheduler." });
            }

            ITrigger _Trigger = TriggerBuilder.Create()
                .WithIdentity("emailTrigger", "elmsJobs")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
                .Build();

            await __Scheduler.ScheduleJob(_Job, _Trigger);

            return RedirectToAction("Index", "Job", new { Area = "Admin", successMessage = $"Successfully triggered Email job scheduler." });
        }
    }
}
