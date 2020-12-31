using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace HangFire.Example.Jobs
{
    public class StartFileExplorer : IRecurringJob
    {
        public string CronTime { get; }
        public string JobName { get; } = "Start File Explorer";

        public StartFileExplorer()
        {
            CronTime = Cron.Hourly(0);

        }
        public void ExecuteJob()
        {
            try
            {
                Process.Start("explorer");
            }
            catch (Exception e)
            {
                // Log /Email whatever you wanna do 
            }

        }
    }
}
