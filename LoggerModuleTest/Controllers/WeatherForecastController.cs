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
        private readonly NLog.Logger _nlgger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, NLog.Logger nlogger)
        {
            _logger = logger;
            _nlgger = nlogger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => {
                var weather = new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
                
                _logger.LogInformation("一周天气预报，星期{index}天气为：{@weather}", index, weather);
                //_nlgger.Info("NLog;一周天气预报，星期{index}天气为：{@weather}", index, weather);
                return weather;
            })
            .ToArray();
        }
        [HttpGet("error")]
        public async Task<bool> ErrorHandle()
        {
            throw new Exception(HttpContext.Request.Path.Value);
            //return await Task.FromResult(true);
        }
    }
}
