using ELMS.WEB.Areas.Admin.Models.Job;
using ELMS.WEB.Jobs;
using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Models.Admin.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class JobController : Controller
    {
        private readonly IScheduler __Scheduler;
        private readonly IConfigurationManager __ConfigurationManager;
        private readonly IConfiguration __Configuration;
        private readonly int DEFAULT_INTERVAL_HOURS = 1;

        public JobController(IScheduler scheduler, IConfigurationManager configurationManager, IConfiguration configuration)
        {
            __Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            __ConfigurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            __Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        [Authorize(Policy = "RunJobSchedulerPolicy")]
        public async Task<IActionResult> Index(string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }
            else if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            return View(new IndexViewModel
            {
                JobRunningStatuses = await GetStatuses()
            });
        }

        [HttpGet]
        [Authorize(Policy = "RunJobSchedulerPolicy")]
        public async Task<IActionResult> StartEquipmentJobAsync()
        {
            IJobDetail _Job = JobBuilder.Create<EquipmentJob>()
                .WithIdentity("equipment", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_Job.Key))
            {
                return RedirectToAction("Index", "Job", new { Area = "Admin", errorMessage = $"Already triggered Equipment job scheduler." });
            }

            ConfigurationResponse _IntervalResponse = await __ConfigurationManager.GetByNormalizedNameAsync(__Configuration.GetValue<string>("Configuration:Equipment:EQUIPMENT_EXPIRY_INTERVAL"));

            if (int.TryParse(_IntervalResponse.Value, out int _IntervalHours))
            {
                ITrigger _Trigger = TriggerBuilder.Create()
                    .WithIdentity("equipmentTrigger", "elmsJobs")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(_IntervalHours).RepeatForever())
                    .Build();
                await __Scheduler.ScheduleJob(_Job, _Trigger);
            }
            else
            {
                ITrigger _Trigger = TriggerBuilder.Create()
                    .WithIdentity("equipmentTrigger", "elmsJobs")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(DEFAULT_INTERVAL_HOURS).RepeatForever())
                    .Build();
                await __Scheduler.ScheduleJob(_Job, _Trigger);
            }

            return RedirectToAction("Index", "Job", new { Area = "Admin", successMessage = $"Successfully triggered Equipment job scheduler." });
        }

        [HttpGet]
        [Authorize(Policy = "RunJobSchedulerPolicy")]
        public async Task<IActionResult> StartLoanJobAsync()
        {
            IJobDetail _Job = JobBuilder.Create<LoanJob>()
                .WithIdentity("loan", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_Job.Key))
            {
                return RedirectToAction("Index", "Job", new { Area = "Admin", errorMessage = $"Already triggered Loan job scheduler." });
            }

            ConfigurationResponse _IntervalResponse = await __ConfigurationManager.GetByNormalizedNameAsync(__Configuration.GetValue<string>("Configuration:General:SCHEDULED_EMAIL_INTERVAL"));

            if (int.TryParse(_IntervalResponse.Value, out int _IntervalHours))
            {
                ITrigger _Trigger = TriggerBuilder.Create()
                    .WithIdentity("loanTrigger", "elmsJobs")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(_IntervalHours).RepeatForever())
                    .Build();
                await __Scheduler.ScheduleJob(_Job, _Trigger);
            }
            else
            {
                ITrigger _Trigger = TriggerBuilder.Create()
                    .WithIdentity("loanTrigger", "elmsJobs")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(DEFAULT_INTERVAL_HOURS).RepeatForever())
                    .Build();
                await __Scheduler.ScheduleJob(_Job, _Trigger);
            }

            return RedirectToAction("Index", "Job", new { Area = "Admin", successMessage = $"Successfully triggered Loan job scheduler." });
        }

        [HttpGet]
        [Authorize(Policy = "RunJobSchedulerPolicy")]
        public async Task<IActionResult> StartEmailJobAsync()
        {
            IJobDetail _Job = JobBuilder.Create<EmailJob>()
                .WithIdentity("email", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_Job.Key))
            {
                return RedirectToAction("Index", "Job", new { Area = "Admin", errorMessage = $"Already triggered Email job scheduler." });
            }

            ConfigurationResponse _IntervalResponse = await __ConfigurationManager.GetByNormalizedNameAsync(__Configuration.GetValue<string>("Configuration:Loan:LOAN_EXPIRY_INTERVAL"));

            if (int.TryParse(_IntervalResponse.Value, out int _IntervalHours))
            {
                ITrigger _Trigger = TriggerBuilder.Create()
                    .WithIdentity("emailTrigger", "elmsJobs")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(_IntervalHours).RepeatForever())
                    .Build();
                await __Scheduler.ScheduleJob(_Job, _Trigger);
            }
            else
            {
                ITrigger _Trigger = TriggerBuilder.Create()
                    .WithIdentity("emailTrigger", "elmsJobs")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(DEFAULT_INTERVAL_HOURS).RepeatForever())
                    .Build();
                await __Scheduler.ScheduleJob(_Job, _Trigger);
            }

            return RedirectToAction("Index", "Job", new { Area = "Admin", successMessage = $"Successfully triggered Email job scheduler." });
        }

        private async Task<JobRunningStatuses> GetStatuses()
        {
            JobRunningStatuses _Statuses = new JobRunningStatuses();

            IJobDetail _EquipmentJob = JobBuilder.Create<EquipmentJob>()
                .WithIdentity("equipment", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_EquipmentJob.Key))
            {
                _Statuses.IsEquipmentJobRunning = true;
            }

            IJobDetail _LoanJob = JobBuilder.Create<LoanJob>()
                .WithIdentity("loan", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_LoanJob.Key))
            {
                _Statuses.IsLoanJobRunning = true;
            }

            IJobDetail _EmailJob = JobBuilder.Create<EmailJob>()
                .WithIdentity("email", "elmsJobs")
                .Build();

            if (await __Scheduler.CheckExists(_EmailJob.Key))
            {
                _Statuses.IsEmailJobRunning = true;
            }

            return _Statuses;
        }
    }
}