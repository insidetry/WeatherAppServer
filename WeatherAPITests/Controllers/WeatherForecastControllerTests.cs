using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Controllers.Tests
{
    [TestClass()]
    public class WeatherForecastControllerTests
    {
        [TestMethod()]
        public async Task HourlyTemperatureTestStartsFrom15()
        {
            IWeatherAccess weather = new WeatherAccessImpl(48);
            var hourlyTemperatures = await weather.HourlyTemperature();
            Assert.AreEqual(hourlyTemperatures.FirstOrDefault().Item2, 288);
        }

        [TestMethod()]
        public async Task HourlyTemperatureTestIncreasesByRandomValuesTill6PM()
        {
            IWeatherAccess weather = new WeatherAccessImpl(48);
            var hourlyTemperatures = await weather.HourlyTemperature();
            var prev = hourlyTemperatures.FirstOrDefault();

            foreach (var ht in hourlyTemperatures.Skip(1))
            {
                if (ht.Item1.Hour < 19 && ht.Item1.Hour > 7)
                {
                    if (ht.Item2 < prev.Item2)
                        Assert.Fail("Current " + ht.Item1.Hour.ToString() + " Previous " + prev.Item1.Hour.ToString() + " Difference " + (ht.Item2 - prev.Item2));
                }
                prev = ht;
            }
        }

        [TestMethod()]
        public async Task HourlyTemperatureTestDecreasesByRandomValuesAfter7PM()
        {
            IWeatherAccess weather = new WeatherAccessImpl(48);
            var hourlyTemperatures = await weather.HourlyTemperature();
            var prev = hourlyTemperatures.FirstOrDefault();

            foreach (var ht in hourlyTemperatures.Skip(1))
            {
                if (ht.Item1.Hour > 19 || ht.Item1.Hour < 7)
                {
                    if (ht.Item2 > prev.Item2)
                        Assert.Fail("Current " + ht.Item1.Hour.ToString() + " Previous " + prev.Item1.Hour.ToString() + " Difference " + (ht.Item2 - prev.Item2));
                }
                prev = ht;
            }
        }
    }
}