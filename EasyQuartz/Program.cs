using Microsoft.Extensions.DependencyInjection;
using EasyQuartz.JobFactories;
using EasyQuartz.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace EasyQuartz
{
    class Program
    {
        private static ServiceProvider ServiceProvider { get; set; }
        static void Main(string[] args)
        {
            var start = new Starup();
            start.Start();
            ServiceProvider = start.ServiceProvider;

            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            RunProgram().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        private static async ValueTask RunProgram()
        {
            try
            {
                var jobFactory = ServiceProvider.GetService<InjectionableJobFactory>();

                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);

                var scheduler = await factory.GetScheduler();
                var jobdetail = JobBuilder.Create<HelloJob>()
                    .WithIdentity(nameof(HelloJob), "group1")
                    .Build();


                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x.
                        WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();
                await scheduler.ScheduleJob(jobdetail, trigger);

                await scheduler.Start();

                await Task.Delay(TimeSpan.FromSeconds(60));

                await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }
        }
    }
}
