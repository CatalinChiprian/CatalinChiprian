using Cloudie.Model;
using Cloudie.Store;
using Cloudie.ViewModel;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.CodeDom;

namespace Cloudie.Commands
{
    /// <summary>
    /// Command for handling city check events.
    /// </summary>
    public class CheckedCityCommand : CommandBase
    {
        private readonly WeatherStats _weatherStats;

        private readonly WeatherGraphViewModel _weatherGraphViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckedCityCommand"/> class.
        /// </summary>
        /// <param name="weatherGraphViewModel">The weather graph view model.</param>
        /// <param name="weatherStats">The weather stats.</param>
        public CheckedCityCommand(WeatherGraphViewModel weatherGraphViewModel, WeatherStats weatherStats)
        {
            _weatherStats = weatherStats;
            _weatherGraphViewModel = weatherGraphViewModel;
        }

        /// <summary>
        /// Executes the command with the specified parameter.
        /// </summary>
        /// <param name="parameter">The city data.</param>
        public override async void Execute(object parameter)
        {
            var city = parameter as CityData;
            if (city == null)
                return;

            if (city.IsChecked)
            {
                IList<WeatherData> weatherData = await GetWeatherData(city);
                ClearWeatherDataIfDateFilterChanged();
                var DateFilteredData = FilterDataByDate(weatherData);
                int samplingInterval = Math.Max(DateFilteredData.Count / 150, 1);
                var sampledData = DateFilteredData.Where((value, index) => index % samplingInterval == 0).ToList();
                AddSeriesToWeatherData(city, sampledData);
            }
            else
            {
                RemoveSeriesFromWeatherData(city);
            }
        }

        /// <summary>
        /// Gets the weather data for the specified city.
        /// </summary>
        /// <param name="city">The city data.</param>
        /// <returns>A list of weather data.</returns>
        private async Task<IList<WeatherData>> GetWeatherData(CityData city)
        {
            IList<WeatherData> weatherData;
            if (!_weatherGraphViewModel.CheckedCitiesData.ContainsKey(city.Name) && !(_weatherStats.Data[0].Device_ID == city.Name))
            {
                await _weatherStats.DataLoad(city.Name);
                weatherData = _weatherStats.Data;
                _weatherGraphViewModel.CheckedCitiesData[city.Name] = new List<WeatherData>(weatherData);
            }
            else if ((_weatherStats.Data[0].Device_ID == city.Name) && !(_weatherGraphViewModel.CheckedCitiesData.ContainsKey(city.Name)))
            {
                weatherData = _weatherStats.Data;
                _weatherGraphViewModel.CheckedCitiesData[city.Name] = new List<WeatherData>(weatherData);
            }
            else
            {
                weatherData = _weatherGraphViewModel.CheckedCitiesData[city.Name];
            }
            return weatherData;
        }

        /// <summary>
        /// Clears the weather data if the date filter has changed.
        /// </summary>
        private void ClearWeatherDataIfDateFilterChanged()
        {
            if (_weatherGraphViewModel.DateFilterChanged)
            {
                _weatherGraphViewModel.WeatherTemp.Clear();
                _weatherGraphViewModel.WeatherHum.Clear();
                _weatherGraphViewModel.WeatherIll.Clear();
                _weatherGraphViewModel.WeatherPress.Clear();
                _weatherGraphViewModel.DateFilterChanged = false;
            }
        }

        /// <summary>
        /// Filters the data by date.
        /// </summary>
        /// <param name="weatherData">The weather data.</param>
        /// <returns>A list of filtered weather data.</returns>
        private IList<WeatherData> FilterDataByDate(IList<WeatherData> weatherData)
        {
            if ((_weatherGraphViewModel.SelectedFilter is not WeatherGraphViewModel.UserDateSelection.AllTime) && (_weatherGraphViewModel.SelectedFilter is not WeatherGraphViewModel.UserDateSelection.CustomRange))
            {
                return weatherData.Where(d => d.Date >= DateTime.Now.AddDays(-(int)_weatherGraphViewModel.SelectedFilter)).ToList();
            }
            else if (_weatherGraphViewModel.SelectedFilter is WeatherGraphViewModel.UserDateSelection.CustomRange)
            {
                return weatherData.Where(d => d.Date >= _weatherGraphViewModel.StartDate && d.Date <= _weatherGraphViewModel.EndDate).ToList();
            }
            return weatherData;
        }


        /// <summary>
        /// Adds series to the weather data.
        /// </summary>
        /// <param name="city">The city data.</param>
        /// <param name="sampledData">The sampled data.</param>
        private void AddSeriesToWeatherData(CityData city, IList<WeatherData> sampledData)
        {
            _weatherGraphViewModel.WeatherTemp.Add(CreateSeries(city.Name, city.CityColors, sampledData.Select(d => (d.Date, d.Temperature)), "Temperature"));
            _weatherGraphViewModel.WeatherHum.Add(CreateSeries(city.Name, city.CityColors, sampledData.Select(d => (d.Date, d.Humidity)), "Humidity"));
            _weatherGraphViewModel.WeatherIll.Add(CreateSeries(city.Name, city.CityColors, sampledData.Select(d => (d.Date, d.Ext_Sensor_Value)), "Illumination"));
            _weatherGraphViewModel.WeatherPress.Add(CreateSeries(city.Name, city.CityColors, sampledData.Select(d => (d.Date, d.Pressure)), "Pressure"));
        }

        /// <summary>
        /// Removes series from the weather data.
        /// </summary>
        /// <param name="city">The city data.</param>
        private void RemoveSeriesFromWeatherData(CityData city)
        {
            RemoveSeries(city.Name, _weatherGraphViewModel.WeatherTemp);
            RemoveSeries(city.Name, _weatherGraphViewModel.WeatherHum);
            RemoveSeries(city.Name, _weatherGraphViewModel.WeatherIll);
            RemoveSeries(city.Name, _weatherGraphViewModel.WeatherPress);
        }

        /// <summary>
        /// Removes a series from a series collection.
        /// </summary>
        /// <param name="title">The title of the series to remove.</param>
        /// <param name="seriesCollection">The series collection.</param>
        private void RemoveSeries(string title, SeriesCollection seriesCollection)
        {
            var seriesToRemove = seriesCollection.FirstOrDefault(s => s.Title == title);
            if (seriesToRemove != null)
            {
                seriesCollection.Remove(seriesToRemove);
            }
        }


        /// <summary>
        /// Creates a line series.
        /// </summary>
        /// <param name="title">The title of the series.</param>
        /// <param name="color">The color of the series.</param>
        /// <param name="values">The values of the series.</param>
        /// <returns>A new line series.</returns>
        private LineSeries CreateSeries(string title, Brush color, IEnumerable<(DateTime Date, double Value)> values, string dataType)
        {
            return new LineSeries
            {
                Title = title,
                Values = new ChartValues<double>(values.Select(v => v.Value)),
                PointGeometry = null,
                Stroke = color,
                LabelPoint = point => $"Date: {values.ElementAt(point.Key).Date}, {dataType}: {point.Y}",
                Foreground = color
            };
        }
    }
}