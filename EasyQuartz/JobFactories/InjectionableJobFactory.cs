using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Logging;
using Quartz.Spi;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyQuartz.JobFactories
{
    public class InjectionableJobFactory : IJobFactory
    {
        private static readonly ILogger log = new LoggerFactory().CreateLogger<InjectionableJobFactory>();

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;
            try
            {
                log.LogInformation($"Producing instance of Job '{jobDetail.Key}', class={jobType.FullName}");
                return ObjectUtils.InstantiateType<IJob>(jobType);
            }
            catch (Exception e)
            {
                SchedulerException se = new SchedulerException($"Problem instantiating class '{jobDetail.JobType.FullName}'", e);
                throw e;
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
