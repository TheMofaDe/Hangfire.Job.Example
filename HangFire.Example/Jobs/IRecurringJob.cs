using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Example.Jobs
{
    public interface IRecurringJob
    {
        string CronTime { get; }
        string JobName { get; }
        void ExecuteJob();
    }

}
