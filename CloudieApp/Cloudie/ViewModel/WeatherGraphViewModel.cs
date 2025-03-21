using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Cloudie.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts.Wpf;
using LiveCharts;
using System.Diagnostics;
using System.Windows.Input;
using Cloudie.Store;
using Cloudie.Commands;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using System.Windows;

namespace Cloudie.ViewModel
{

    /// <summary>
    /// ViewModel for the Weather Graph.
    /// </summary>
    public partial class WeatherGraphViewModel : ViewModelBase
    {

        /// <summary>
        /// Gets the checked cities data.
        /// </summary>
        public Dictionary<string, IList<WeatherData>> CheckedCitiesData { get; } = new Dictionary<string, IList<WeatherData>>();

        /// <summary>
        /// Indicates if the date filter has changed.
        /// </summary>
        public bool DateFilterChanged = false;

        /// <summary>
        /// Enum for user date selection.
        /// </summary>
        public enum UserDateSelection
        {
            Last24H = 1,
            LastWeek = 7,
            LastMonth = 30,
            AllTime = 100,
            CustomRange = 1000
        };

        private UserDateSelection _selectedFilter = UserDateSelection.LastWeek;

        /// <summary>
        /// Enum for the selected date.
        /// </summary>
        public UserDateSelection SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                if (value == UserDateSelection.CustomRange)
                {
                    DateVisibility = Visibility.Visible;
                }
                else
                {
                    DateVisibility = Visibility.Collapsed;
                }

                OnPropertyChanged(nameof(SelectedFilter));
                DateFilterChanged = true;
                foreach(var city in Cities)
                {
                    if (city.IsChecked)
                        CheckedCity.Execute(city);
                }
            }
        }

        private DateTime? _startDate;

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                if (value != null && value > EndDate)
                {
                    MessageBox.Show("Start date must be before end date");
                    return;
                }
                if (value != null && value > DateTime.Now)
                {
                    MessageBox.Show("Start date must be before current date");
                    return;
                }
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                DateFilterChanged = true;
                foreach (var city in Cities)
                {
                    if (city.IsChecked)
                        CheckedCity.Execute(city);
                }
            }
        }

        private DateTime? _endDate;

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (value != null && value < StartDate)
                {
                    MessageBox.Show("End date must be after start date");
                    return;
                }
                if (value != null && value > DateTime.Now)
                {
                    MessageBox.Show("End date must be before current date");
                    return;
                }
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                DateFilterChanged = true;
                foreach (var city in Cities)
                {
                    if (city.IsChecked)
                        CheckedCity.Execute(city);
                }
            }
        }

        private Visibility _dateVisibility = Visibility.Collapsed;

        public Visibility DateVisibility
        {
            get { return _dateVisibility; }
            set
            {
                _dateVisibility = value;
                OnPropertyChanged(nameof(DateVisibility));
            }
        }

        public Array UserDateSelectionValues
        {
            get { return Enum.GetValues(typeof(UserDateSelection)); }
        }
        public Func<double, string> YFormatter { get; set; }

        public readonly WeatherStats weatherS;

        private double _actualHeight;

        public double ActualHeight
        {
            get { return _actualHeight; }
            set
            {
                _actualHeight = value;
                OnPropertyChanged(nameof(ActualHeight));
                OnPropertyChanged(nameof(GraphHeight));
            }
        }

        public double GraphHeight
        {
            get
            {
                int visibleGraphCount = new[] { WeatherPressVisible, WeatherTempVisible }.Count(v => v);
                return visibleGraphCount > 0 ? ActualHeight / visibleGraphCount : ActualHeight;
            }
        }


        private double _actualWidth;
        public double ActualWidth
        {
            get { return _actualWidth; }
            set
            {
                _actualWidth = value;
                OnPropertyChanged(nameof(ActualWidth));
                OnPropertyChanged(nameof(GraphWidth));
            }
        }
        public double GraphWidth
        {
            get
            {
                int visibleGraphCount = new[] { WeatherPressVisible, WeatherTempVisible }.Count(v => v);
                return visibleGraphCount > 0 ? ActualWidth / visibleGraphCount : ActualWidth;
            }
        }

        private int graphCount;

        private List<int> _tempGraphRowColSpan;
        public List<int> TempGraphRowColSpan
        {
            get { return _tempGraphRowColSpan; }
            set
            {
                _tempGraphRowColSpan = value;
                OnPropertyChanged(nameof(TempGraphRowColSpan));
            }
        }

        private List<int> _humGraphRowColSpan;

        public List<int> HumGraphRowColSpan
        {
            get { return _humGraphRowColSpan; }
            set
            {
                _humGraphRowColSpan = value;
                OnPropertyChanged(nameof(HumGraphRowColSpan));
            }
        }

        private List<int> _illGraphRowColSpan;

        public List<int> IllGraphRowColSpan
        {
            get { return _illGraphRowColSpan; }
            set
            {
                _illGraphRowColSpan = value;
                OnPropertyChanged(nameof(IllGraphRowColSpan));
            }
        }

        private List<int> _pressGraphRowColSpan;

        public List<int> PressGraphRowColSpan
        {
            get { return _pressGraphRowColSpan; }
            set
            {
                _pressGraphRowColSpan = value;
                OnPropertyChanged(nameof(PressGraphRowColSpan));
            }
        }

        private List<int> _tempGraphCoordonates;

        public List<int> TempGraphCoordonates
        {
            get { return _tempGraphCoordonates; }
            set
            {
                _tempGraphCoordonates = value;
                OnPropertyChanged(nameof(TempGraphCoordonates));
            }
        }

        private List<int> _humGraphCoordonates;

        public List<int> HumGraphCoordonates
        {
            get { return _humGraphCoordonates; }
            set
            {
                _humGraphCoordonates = value;
                OnPropertyChanged(nameof(HumGraphCoordonates));
            }
        }

        private List<int> _illGraphCoordonates;

        public List<int> IllGraphCoordonates
        {
            get { return _illGraphCoordonates; }
            set
            {
                _illGraphCoordonates = value;
                OnPropertyChanged(nameof(IllGraphCoordonates));
            }
        }

        private List<int> _pressGraphCoordonates;

        public List<int> PressGraphCoordonates
        {
            get { return _pressGraphCoordonates; }
            set
            {
                _pressGraphCoordonates = value;
                OnPropertyChanged(nameof(PressGraphCoordonates));
            }
        }

        private bool _weatherTempVisible;

        private bool _weatherHumVisible;

        private bool _weatherIllVisible;

        private bool _weatherPressVisible;

        public bool WeatherTempVisible
        {
            get { return _weatherTempVisible; }
            set
            {
                _weatherTempVisible = value;
                OnPropertyChanged(nameof(WeatherTempVisible));
                OnPropertyChanged(nameof(GraphWidth));
                OnPropertyChanged(nameof(GraphHeight));
            }
        }

        public bool WeatherHumVisible
        {
            get { return _weatherHumVisible; }
            set
            {
                _weatherHumVisible = value;
                OnPropertyChanged(nameof(WeatherHumVisible));
                OnPropertyChanged(nameof(GraphWidth));
                OnPropertyChanged(nameof(GraphHeight));
            }
        }

        public bool WeatherIllVisible
        {
            get { return _weatherIllVisible; }
            set
            {
                _weatherIllVisible = value;
                OnPropertyChanged(nameof(WeatherIllVisible));
                OnPropertyChanged(nameof(GraphWidth));
                OnPropertyChanged(nameof(GraphHeight));
            }
        }

        public bool WeatherPressVisible
        {
            get { return _weatherPressVisible; }
            set
            {
                _weatherPressVisible = value;
                OnPropertyChanged(nameof(WeatherPressVisible));
                OnPropertyChanged(nameof(GraphWidth));
                OnPropertyChanged(nameof(GraphHeight));
            }
        }


        private SeriesCollection _weatherTemp;

        public SeriesCollection WeatherTemp
        {
            get { return _weatherTemp; }
            set
            {
                _weatherTemp = value;
                OnPropertyChanged(nameof(WeatherTemp));
            }
        }

        private SeriesCollection _weatherHum;

        public SeriesCollection WeatherHum
        {
            get { return _weatherHum; }
            set
            {
                _weatherHum = value;
                OnPropertyChanged(nameof(WeatherHum));
            }
        }

        private SeriesCollection _weatherIll;

        public SeriesCollection WeatherIll
        {
            get { return _weatherIll; }
            set
            {
                _weatherIll = value;
                OnPropertyChanged(nameof(WeatherIll));
            }
        }

        private SeriesCollection _weatherPress;

        public SeriesCollection WeatherPress
        {
            get { return _weatherPress; }
            set
            {
                _weatherPress = value;
                OnPropertyChanged(nameof(WeatherPress));
            }
        }

        private ObservableCollection<string> _labels;

        public ObservableCollection<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

        public ICommand ToStats { get; }
        public ICommand ToGraphs { get; }
        public ICommand CheckedCity { get; }
        public ICommand ToDeviceInfo { get; }

        private string _city;

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private ObservableCollection<CityData> _cities;

        public ObservableCollection<CityData> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        /// <summary>
        /// Takes all the cities from the Cities Collection and formats them after the CityData.
        /// </summary>
        public void GraphsPerCity()
        {
            YFormatter = value => value.ToString("0");
            Cities = new ObservableCollection<CityData>(weatherS.Cities.Select(city => new CityData { Name = city }));
            List<Brush> colors = new List<Brush> { Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Yellow, Brushes.AliceBlue, Brushes.Orange, Brushes.Navy, Brushes.Turquoise };
            int colorIndex = 0;

            foreach (var city in Cities)
            {
                city.IsChecked = city.Name == City;
                city.CityColors = colors[colorIndex];
                colorIndex = (colorIndex + 1) % colors.Count;// This will loop back to the start of the colors list when it reaches the end
                if (city.IsChecked)
                    CheckedCity.Execute(city);
            }
        }

        /// <summary>
        /// Constructor for WeatherGraphViewModel.
        /// </summary>
        /// <param name="navigationStore">The navigation store.</param>
        /// <param name="weatherStats">The weather stats.</param>
        public WeatherGraphViewModel(NavigationStore navigationStore, WeatherStats weatherStats)
        {
            weatherS = weatherStats;
            City = weatherStats.SelectedCity;
            WeatherTempVisible = true;
            WeatherHumVisible = true;
            WeatherIllVisible = true;
            WeatherPressVisible = true;

            TempGraphCoordonates = new List<int> { 0, 0 };
            TempGraphRowColSpan = new List<int> { 1, 1 };

            HumGraphCoordonates = new List<int> { 0, 1 };
            HumGraphRowColSpan = new List<int> { 1, 1 };

            IllGraphCoordonates = new List<int> { 1, 0 };
            IllGraphRowColSpan = new List<int> { 1, 1 };

            PressGraphCoordonates = new List<int> { 1, 1 };
            PressGraphRowColSpan = new List<int> { 1, 1 };

            graphCount = 4;

            AddDummyGraph();
            CheckedCity = new CheckedCityCommand(this, weatherStats);
            ToStats = new ToStatsCommand(navigationStore, weatherStats);
            ToGraphs = new ToGraphsCommand(navigationStore, weatherStats);
            ToDeviceInfo = new ToDeviceInfoCommand(navigationStore, weatherStats);
            GraphsPerCity();
        }

        /// <summary>
        /// Adds a dummy graph.
        /// </summary>
        private void AddDummyGraph()
        {
            WeatherTemp = new SeriesCollection { };

            WeatherPress = new SeriesCollection { };

            WeatherHum = new SeriesCollection { };

            WeatherIll = new SeriesCollection { };

            Labels = new ObservableCollection<string> { };
        }
    }
}

