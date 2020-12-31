using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace HangFire.Example.Jobs
{
    public class StartCalculatorJob : IRecurringJob
    {
        public string CronTime { get; }
        public string JobName { get; } = "Start Calculator";

        public StartCalculatorJob()
        {
            CronTime = Cron.Daily(3,0);

        }
        public void ExecuteJob()
        {
            try
            {
                Process.Start("calc.exe");
            }
            catch (Exception)
            {
                // Log /Email whatever you wanna do 
            }

        }
    }
}
