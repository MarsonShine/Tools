using LoggerModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggerModuleTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            _logger.LogInformation("一周天气预报，星期{index}天气为：{@weather}", 1, "好天气");
            //_logger.Log(LogLevel.Information, default, new MSLoggerEvent("一周天气预报，星期{index}天气为：{@weather}", 2, "坏天气").WithProperty("elapsedTime", getRequestElasedTime()), null, MSLoggerEvent.Formatter);
            return Enumerable.Range(1, 5).Select(index => {
                var weather = new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
                return weather;
            })
            .ToArray();
        }
        string getRequestElasedTime()
        {
            if (HttpContext != null)
            {
                var r = HttpContext.Items.TryGetValue("ElapsedTime", out object val);
                if (r)
                {
                    var sw = (val as System.Diagnostics.Stopwatch);
                    if (sw != null)
                    {
                        sw.Stop();
                    }
                    return sw.ElapsedMilliseconds + "ms";
                }
            }
            return "";
        }
        [HttpGet("error")]
        public async Task<bool> ErrorHandle()
        {
            throw new Exception(HttpContext.Request.Path.Value);
            //return await Task.FromResult(true);
        }
    }
}
