using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace HangFire.Example.Jobs
{
    public class AlwaysFailJob : IRecurringJob
    {
        public string CronTime { get; }
        public string JobName { get; } = "Always Fail";

        public AlwaysFailJob()
        {
            CronTime = Cron.Minutely();

        }
        public void ExecuteJob()
        {
            throw new Exception("I failed on purpose");

        }
    }
}
