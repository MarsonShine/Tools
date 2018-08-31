using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyQuartz.Jobs
{
    public class SomeScopedJob : IJob
    {
        private readonly IServiceProvider m_serviceProvider;

        public SomeScopedJob(IServiceProvider serviceProvider) {
            m_serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync(nameof(SomeScopedJob));
        }
    }
}
