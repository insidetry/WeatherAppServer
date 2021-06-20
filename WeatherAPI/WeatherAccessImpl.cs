using System.Collections.Generic;
using System;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

public interface IWeatherAccess
{
    Task<IEnumerable<(DateTime, double)>> HourlyTemperature();
}

public class WeatherAccessImpl : IWeatherAccess
{
    private readonly Random _random = new Random();
    private int Count { get; set; }

    public WeatherAccessImpl(int count)
    {
        Count = count;
    }

    public Task<IEnumerable<(DateTime, double)>> HourlyTemperature()
    {
        return Task.FromResult(Calculate());
    }

    private IEnumerable<(DateTime, double)> Calculate()
    {
        double temperature = 15;
        var currentDate = DateTime.Now;
        for (int i = 0; i < Count; i++)
        {
            var tempInK = GetTemperatureInK(temperature);
            yield return (currentDate, tempInK);
            currentDate = currentDate.AddHours(1);
            temperature = GetNextTemperatureInC(currentDate, temperature);
        }

    }

    private double GetNextTemperatureInC(DateTime currentdate, double temp)
    {
        var increment = GetRandomTemperatureIncrement();
        double tempInCelsius = 0;
        if (currentdate.Hour > 7 && currentdate.Hour < 19)
            tempInCelsius = temp + increment;
        else
            tempInCelsius = temp - increment;


        return tempInCelsius;
    }

    private double GetRandomTemperatureIncrement()
    {
        return GetRandomNumber(0, 0.5);
    }

    private double GetRandomNumber(double minimum, double maximum)
    {
        Random random = new Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    private double GetTemperatureInK(double temp)
    {
        return temp + 273;
    }

    private double GetTemperatureInC(double temp)
    {
        return temp - 273;
    }

}

namespace WeatherAPI
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        IWeatherAccess weather = new WeatherAccessImpl(48);
    //        Print(weather);
    //        DumpInCsv("D:\\WeatherDump.csv", weather, 10);
    //    }

    //    static async void Print(IWeatherAccess weather)
    //    {
    //        var hourlyTemperatures = await weather.HourlyTemperature();
    //        foreach (var ht in hourlyTemperatures)
    //        {
    //            Console.WriteLine(ht.Item1 + "   " + ht.Item2);
    //        }
    //    }

    //    static async void DumpInCsv(string file, IWeatherAccess weather, int numOfTimes)
    //    {
    //        var hourlyTemperatures = await weather.HourlyTemperature();
    //        using (var writer = new StreamWriter(file))
    //        {
    //            foreach (var ht in hourlyTemperatures)
    //            {
    //                writer.WriteLine(ht.Item1 + "," + ht.Item2);
    //            }
    //        }
    //    }
    //}
}
