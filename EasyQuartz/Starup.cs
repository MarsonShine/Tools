using EasyQuartz.JobFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyQuartz
{
    public class Starup
    {
        public IServiceProvider Start()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient<IJobFactory>(sp => new InjectionableJobFactory(sp))
                .AddScoped<ISchedulerFactory>(sp => new StdSchedulerFactory())
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
