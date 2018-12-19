using EasyQuartz.Jobs;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider m_serviceProvider;
        public InjectionableJobFactory(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;
            try
            {
                log.LogInformation($"Producing instance of Job '{jobDetail.Key}', class={jobType.FullName}");
                //return ObjectUtils.InstantiateType<IJob>(jobType);
                return new SomeScopedJob(m_serviceProvider);
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
