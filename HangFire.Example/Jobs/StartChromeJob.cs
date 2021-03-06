﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetHelper_CommandLine;
using Hangfire;

namespace HangFire.Example.Jobs
{
    public class StartChromeJob : IRecurringJob
    {
        public string CronTime { get; }
        public string JobName { get; } = "Start Chrome ";

        public StartChromeJob()
        {
            CronTime = Cron.Hourly(13);

        }
        public void ExecuteJob()
        {
            try
            {

                var cmd = new CommandPrompt();
                 cmd.RunCommand("chrome.exe https://devblogs.microsoft.com/");

            }
            catch (Exception e)
            {
                // Log /Email whatever you wanna do 
            }

        }
    }
}
