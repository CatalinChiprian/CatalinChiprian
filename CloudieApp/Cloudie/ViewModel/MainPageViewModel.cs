using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cloudie.Model;
using Cloudie.Commands;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using System.ComponentModel;
using Cloudie.Store;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace Cloudie.ViewModel
{

    /// <summary>
    /// Represents the main page view model.
    /// </summary>
    public partial class MainPageViewModel : ViewModelBase
    {
        // Constants for icon paths and text labels
        private const string winterIconPath = @"/assets/weathericons/snow.png";
        private const string sunIconPath = @"/assets/weathericons/sun.png";
        private const string cloudyIconPath = @"/assets/weathericons/cloudy.png";
        private const string AirPressureText = "Air Pressure: ";
        private const string HumidityText = "Humidity: ";
        private const string VisibilityText = "Visibility: ";


        // Properties for UI data binding

        private ImageSource? _png;
        public ImageSource? Png
        {
            get { return _png; }
            set
            {
                _png = value;
                OnPropertyChanged(nameof(Png));
            }
        }

        private string? _date;
        public string? Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string? _hour;
        public string? Hour
        {
            get { return _hour; }
            set
            {
                _hour = value;
                OnPropertyChanged(nameof(Hour));
            }
        }

        private string? _city;
        public string? City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
                UpdateCityData(_city);
                weatherS.SelectedCity = _city;
            }
        }

        private string? _pressure;
        public string? Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                OnPropertyChanged(nameof(Pressure));
            }
        }

        private string? _humidity;
        public string? Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        private string? _temperature;
        public string? Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }


        private string? _visibility;
        public string? Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        private ObservableCollection<string> _cities;
        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private ObservableCollection<WeatherData> _averageData;
        public ObservableCollection<WeatherData> AverageData
        {
            get { return _averageData; }
            set
            {
                _averageData = value;
                OnPropertyChanged(nameof(AverageData));
            }
        }


        private ObservableCollection<WeatherData> _data;
        public ObservableCollection<WeatherData> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private ObservableCollection<WeatherHistory> _weatherHistoryView;
        public ObservableCollection<WeatherHistory> WeatherHistoryView
        {
            get { return _weatherHistoryView; }
            set
            {
                _weatherHistoryView = value;
                OnPropertyChanged(nameof(WeatherHistoryView));
            }
        }


        private SeriesCollection _weatherStats;
        public SeriesCollection WeatherStats
        {
            get { return _weatherStats; }
            set
            {
                _weatherStats = value;
                OnPropertyChanged(nameof(WeatherStats));
            }
        }

        public readonly WeatherStats weatherS;

       // Commands for UI actions
        public ICommand ToStats { get; }
        public ICommand ToGraphs { get; }
        public ICommand ToDeviceInfo { get; }

        private IList<string> _labels;
        public IList<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }


        /// <summary>
        /// Gets the day suffix for a given day.
        /// </summary>
        /// <param name="day">The day of the month.</param>
        /// <returns>The suffix for the day.</returns>
        public static string GetDaySuffix(int day)
        {
            if (day % 10 == 1 && day != 11)
            {
                return "st";
            }
            else if (day % 10 == 2 && day != 12)
            {
                return "nd";
            }
            else if (day % 10 == 3 && day != 13)
            {
                return "rd";
            }
            else
            {
                return "th";
            }
        }


        /// <summary>
        /// Clears the data.
        /// </summary>
        private void ClearData()
        {
            _weatherHistoryView.Clear();
            Date = "";
            Hour = "";
            Pressure = "";
            Humidity = "";
            Temperature = "";
            Visibility = "";
            Png = null;
        }

        /// <summary>
        /// Updates the properties.
        /// </summary>
        private void UpdateProperties()
        {
            var date = Data.Last().Date;
            Date = string.Format("{0:ddd, MMM}, {1}{2}", date, date.Day, GetDaySuffix(date.Day));
            Hour = Data.Last().Date.ToString("HH:mm");
            Pressure = AirPressureText + Data.Last().Pressure.ToString(CultureInfo.InvariantCulture) + " inHg";
            Humidity = HumidityText + Data.Last().Humidity.ToString(CultureInfo.InvariantCulture) + "%";
            Png = GetWeatherIcon(Data.Last().Temperature);
            Temperature = Math.Round(Data.Last().Temperature, 1).ToString(CultureInfo.InvariantCulture) + "°C";
            Visibility = VisibilityText + Data.Last().Ext_Sensor_Value.ToString(CultureInfo.InvariantCulture) + " %";
        }

        /// <summary>
        /// Gets the weather icon based on the temperature.
        /// </summary>
        /// <param name="temperature">The temperature.</param>
        /// <returns>The weather icon.</returns>
        private ImageSource GetWeatherIcon(double temperature)
        {
            return new BitmapImage(new Uri(
                temperature <= 0.5 ? winterIconPath :
                temperature < 7 ? cloudyIconPath :
                sunIconPath, UriKind.Relative));
        }

        /// <summary>
        /// Updates the city data.
        /// </summary>
        /// <param name="city">The city.</param>
        public async void UpdateCityData(string city)
        {
            // Clear the list before adding new data
            ClearData();
            await weatherS.DataLoad(city);
            AddAverageGraph();
            // If there is nothing in the database, return
            if (Data.Count == 0) return;
            // Update the properties
            UpdateProperties();
            // Update other properties if needed
            AddStats();
        }


        /// <summary>
        /// Initializes a new instance of the MainPageViewModel class.
        /// </summary>
        /// <param name="navigationStore">The navigation store.</param>
        /// <param name="weatherStats">The weather stats.</param>
        public MainPageViewModel(NavigationStore navigationStore, WeatherStats weatherStats)
        {
            ToStats = new ToStatsCommand(navigationStore, weatherStats);
            ToGraphs = new ToGraphsCommand(navigationStore, weatherStats);
            ToDeviceInfo = new ToDeviceInfoCommand(navigationStore, weatherStats);
            _weatherHistoryView = new ObservableCollection<WeatherHistory>();
            weatherS = weatherStats;
            AverageData = weatherS.AverageData;
            Data = (ObservableCollection<WeatherData>)weatherS.Data;
            Cities = weatherS.Cities;
            City = Cities.FirstOrDefault();
        }


        /// <summary>
        /// Adds the stats.
        /// </summary>
        private void AddStats()
        {
            foreach (var data in AverageData)
            {
                if (data.AvgTemp < 0) WeatherHistoryView.Add(new WeatherHistory(data.AvgTemp, winterIconPath, data.Date));
                else if (data.AvgTemp < 7) WeatherHistoryView.Add(new WeatherHistory(data.AvgTemp, cloudyIconPath, data.Date));
                else
                    WeatherHistoryView.Add(new WeatherHistory(data.AvgTemp, sunIconPath, data.Date));
            }
            WeatherHistoryView.Reverse();
        }

        /// <summary>
        /// Adds the average graph.
        /// </summary>
        private void AddAverageGraph()
        {
            var temperatureValues = new ChartValues<double>();
            foreach (var data in AverageData)
            {
                temperatureValues.Add(Math.Round(data.AvgTemp,2));
            }
            WeatherStats = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Average Temperature",
                    Values = temperatureValues,
                    PointGeometry = DefaultGeometries.Circle,
                    LabelPoint = point => $"{point.Y} °C",
                    Foreground = Brushes.Red
                }
            };

            var systemTime = DateTime.Now;
            Labels = new List<string>();
            for (int i = 0; i < 12; ++i)
                Labels.Add(systemTime.AddHours(-i).ToShortTimeString());
        }
    }
}
