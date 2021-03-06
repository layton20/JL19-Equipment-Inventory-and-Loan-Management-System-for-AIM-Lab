using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Jobs
{
    public class EquipmentJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            string message = "Simple job execution";
            Debug.WriteLine(message);
        }
    }
}
