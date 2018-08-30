using EasyQuartz.JobFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyQuartz
{
    public class Starup
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public void Start()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient<IJobFactory, InjectionableJobFactory>()
                .BuildServiceProvider();

            ServiceProvider = serviceProvider;
        }
    }
}
