using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;

namespace ELMS.WEB.Jobs
{
    public class AspNetCoreJobFactory : SimpleJobFactory
    {
        private readonly IServiceProvider __ServiceProvider;

        public AspNetCoreJobFactory(IServiceProvider serviceProvider)
        {
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)__ServiceProvider.GetService(bundle.JobDetail.JobType);
            }
            catch (Exception e)
            {
                throw new SchedulerException($"Problem while instantiating job '{bundle.JobDetail.Key}' from the AspNet Core IOC.");
            }
        }
    }
}
