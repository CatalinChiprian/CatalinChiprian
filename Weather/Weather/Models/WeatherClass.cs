using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using System.Text.Json;
namespace Weather.Models;


public class WeatherClass
{
    public string Symbol { get; set; } = "c";
    public string CitySearch { get; set; }
    public string CityName { get; set; }
    public string Country { get; set; }
    public string Png { get; set; }
    public string Temperature { get; set; }
    public int CurrentDay { get; set; } = 0;
    public ObservableCollection<string> HourForecastTime { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> HourForecastTemperature { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> HourForecastPng { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> DayName { get; } = new ObservableCollection<string>();
    public ObservableCollection<string> DayIcon { get; } = new ObservableCollection<string>();
    public ObservableCollection<string> DayMin { get; } = new ObservableCollection<string>();
    public ObservableCollection<string> DayMax { get; } = new ObservableCollection<string>();
    private JsonDocument JsonDocument { get; set; }

    public async Task Setup(string city)
    {
        if (city.Length != 0)
        {
            var client = new HttpClient();
            var baseurl =
                $"https://api.weatherapi.com/v1/forecast.json?key=3bcf288382334aebb93151712242404&q={city}&days=14";
            var response = client.GetAsync(baseurl).Result;
            if (response.StatusCode == (HttpStatusCode)400)
                throw new Exception("Invalid location!");
            CitySearch = city;
            var text = response.Content.ReadAsStringAsync().Result;
            JsonDocument = JsonDocument.Parse(text);
        }
    }

    public void Forecast()
    {
        Png = "https:" + JsonDocument.RootElement.GetProperty("current").GetProperty("condition")
            .GetProperty("icon").GetString();
        CityName = JsonDocument.RootElement.GetProperty("location").GetProperty("name").GetString();
        Country = JsonDocument.RootElement.GetProperty("location").GetProperty("country").GetString();
        Temperature =
            JsonDocument.RootElement.GetProperty("current").GetProperty($"temp_{Symbol}").GetDouble().ToString() +
            "°" + Symbol.ToUpper();
        var forecastday = JsonDocument.RootElement.GetProperty("forecast").GetProperty("forecastday");
        if (DayName.Count > 0)
        {
            DayName.Clear();
            DayIcon.Clear();
            DayMin.Clear();
            DayMax.Clear();
        }

        for (var i = 0; i < forecastday.GetArrayLength(); i++)
        {
            var daystring = forecastday[i].GetProperty("date").GetString();
            DateTime date = DateTime.Parse(daystring);
            string day;
            day = date.ToString(i == 0 ? "dddddd" : "ddd");
            DayName.Add(day);
            DayIcon.Add("https:" + forecastday[i].GetProperty("day").GetProperty("condition").GetProperty("icon")
                .GetString());
            DayMin.Add(forecastday[i].GetProperty("day").GetProperty($"mintemp_{Symbol}").GetDouble().ToString() +
                       "°");
            DayMax.Add(forecastday[i].GetProperty("day").GetProperty($"maxtemp_{Symbol}").GetDouble().ToString() +
                       "°");
        }

        int dayForecast = 0;
        if (HourForecastTime.Count > 0)
        {
            HourForecastTemperature.Clear();
            HourForecastTime.Clear();
            HourForecastPng.Clear();
        }

        while (HourForecastTime.Count != 24)
        {
            DateTime sunrise, sunset;
            var localTime =
                DateTime.ParseExact(
                    JsonDocument.RootElement.GetProperty("location").GetProperty("localtime").GetString(),
                    "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture);
            if (dayForecast != 0)
            {
                localTime = localTime.AddDays(dayForecast);
                localTime = localTime.AddHours(-localTime.Hour);
                localTime = localTime.AddMinutes(-localTime.Minute);
            }

            var hourArray = forecastday[dayForecast].GetProperty("hour");

            var sunrisetest = DateTime.ParseExact(
                forecastday[dayForecast].GetProperty("astro").GetProperty("sunrise").GetString(),
                "hh:mm tt",
                CultureInfo.InvariantCulture);
            sunrise = sunrisetest.Hour >= localTime.Hour ? sunrisetest : localTime.AddHours(-1);

            var sunsettest = DateTime.ParseExact(
                forecastday[dayForecast].GetProperty("astro").GetProperty("sunset").GetString(),
                "hh:mm tt",
                CultureInfo.InvariantCulture);
            sunset = sunsettest.Hour >= localTime.Hour ? sunsettest : localTime.AddHours(-1);
            for (var i = localTime.Hour; i < hourArray.GetArrayLength(); i++)
            {
                if (HourForecastTemperature.Count == 24)
                    break;
                var time = DateTime.ParseExact(hourArray[i].GetProperty("time").GetString(), "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture);
                if (time.Hour >= localTime.Hour)
                {
                    if (sunrise.Hour == time.Hour)
                    {
                        HourForecastTemperature.Add("Sunrise");
                        HourForecastTime.Add(sunrise.ToString("HH:mm"));
                        HourForecastPng.Add("https://i.ibb.co/rwXXL9Y/sunrise.png");
                    }
                    else if (sunset.Hour == time.Hour)
                    {
                        HourForecastTemperature.Add("Sunset");
                        HourForecastTime.Add(sunset.ToString("HH:mm"));
                        HourForecastPng.Add("https://i.ibb.co/68N8WKK/sunset.png");
                    }
                    else
                    {
                        if (HourForecastTime.Count == 0)
                        {
                            HourForecastTemperature.Add(hourArray[i].GetProperty($"temp_{Symbol}").GetDouble()
                                .ToString() + "°");
                            HourForecastTime.Add("Now");
                        }
                        else
                        {
                            HourForecastTemperature.Add(hourArray[i].GetProperty($"temp_{Symbol}").GetDouble()
                                .ToString() + "°");
                            HourForecastTime.Add(time.ToString("HH"));
                        }

                        HourForecastPng.Add("https:" +
                                            hourArray[i].GetProperty("condition").GetProperty("icon").GetString());
                    }
                }
            }

            dayForecast += 1;
        }
    }

    public WeatherClass()
    {
    }
}