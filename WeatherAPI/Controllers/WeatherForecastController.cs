using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WeatherAPI.Controllers
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
        public string MyGet()
        {
            IWeatherAccess weather = new WeatherAccessImpl(48);
            IEnumerable<(DateTime, double)> data = null;

            weather.HourlyTemperature().Wait();
            var task = weather.HourlyTemperature();
            Task continuation = task.ContinueWith(t =>
            {
                Console.WriteLine("Result: " + t.Result);
                data = t.Result;
            });
            continuation.Wait();
            Console.WriteLine(data.ToString());
            var newList = new List<DateTemp>();

            foreach(var e in data)
            {
                var d = new DateTemp();
                d.date = e.Item1;
                d.temp = e.Item2;
                newList.Add(d);
            }
            var value = JsonSerializer.Serialize(newList);
            return value; 
        }

        [Serializable]
        class DateTemp
        {
           public DateTime date { get; set; }
            public double temp { get; set; }
        }

    }
}
