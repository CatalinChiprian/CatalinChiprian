using System.Collections.ObjectModel;
using System.Windows.Input;
using Weather.Commands;
using Weather.Models;
using Weather.Store;

namespace Weather.ViewModels;

public class WeatherViewModel : ViewModelBase
{
    public readonly WeatherClass WeatherClassInstance;
    private string _png;

    public string Png
    {
        get => _png;
        set
        {
            _png = value;
            OnPropertyChanged(nameof(Png));
        }
    }

    private string _citySearch;

    public string CitySearch
    {
        get => _citySearch;
        set
        {
            _citySearch = value;
            OnPropertyChanged(nameof(CitySearch));
        }
    }

    private string _cityName;

    public string CityName
    {
        get => _cityName;
        set
        {
            _cityName = value;
            OnPropertyChanged(nameof(CityName));
        }
    }

    private string _country;

    public string Country
    {
        get => _country;
        set
        {
            _country = value;
            OnPropertyChanged(nameof(Country));
        }
    }

    private string _temperature;

    public string Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            OnPropertyChanged(nameof(Temperature));
        }
    }

    private string _symbol;

    public string Symbol
    {
        get => _symbol;
        set
        {
            _symbol = value;
            OnPropertyChanged(nameof(Symbol));
        }
    }

    private ObservableCollection<string> _hourForecastPng;

    public ObservableCollection<string> HourForecastPng
    {
        get => _hourForecastPng;
        set
        {
            _hourForecastPng = value;
            OnPropertyChanged(nameof(HourForecastPng));
        }
    }

    private ObservableCollection<string> _hourForecastTime;

    public ObservableCollection<string> HourForecastTime
    {
        get => _hourForecastTime;
        set
        {
            _hourForecastTime = value;
            OnPropertyChanged(nameof(HourForecastTime));
        }
    }

    private ObservableCollection<string> _hourForecastTemperature;

    public ObservableCollection<string> HourForecastTemperature
    {
        get => _hourForecastTemperature;
        set
        {
            _hourForecastTemperature = value;
            OnPropertyChanged(nameof(HourForecastTemperature));
        }
    }

    private ObservableCollection<string> _dayName;

    public ObservableCollection<string> DayName
    {
        get => _dayName;
        set
        {
            _dayName = value;
            OnPropertyChanged(nameof(DayName));
        }
    }

    private ObservableCollection<string> _dayIcon;

    public ObservableCollection<string> DayIcon
    {
        get => _dayIcon;
        set
        {
            _dayIcon = value;
            OnPropertyChanged(nameof(DayIcon));
        }
    }

    private ObservableCollection<string> _dayMin;

    public ObservableCollection<string> DayMin
    {
        get => _dayMin;
        set
        {
            _dayMin = value;
            OnPropertyChanged(nameof(DayMin));
        }
    }

    private ObservableCollection<string> _dayMax;

    public ObservableCollection<string> DayMax
    {
        get => _dayMax;
        set
        {
            _dayMax = value;
            OnPropertyChanged(nameof(DayMax));
        }
    }

    public ICommand CCommand { get; }
    public ICommand FCommand { get; }
    public ICommand EnterCommand { get; }
    public ICommand MMCommand { get; }

    public WeatherViewModel(NavigationStore navigationStore, WeatherClass weatherClass)
    {
        WeatherClassInstance = weatherClass;
        EnterCommand = new EnterCommand(weatherClass, this);
        CCommand = new CelsiusCommand(weatherClass, EnterCommand);
        FCommand = new FahrenheitCommand(weatherClass, EnterCommand);
        MMCommand = new MainMenuCommand(navigationStore, this);
        PropertiesInit();
    }

    public void PropertiesInit()
    {
        Png = WeatherClassInstance.Png;
        CityName = WeatherClassInstance.CityName;
        CitySearch = WeatherClassInstance.CitySearch;
        Country = WeatherClassInstance.Country;
        Temperature = WeatherClassInstance.Temperature;
        Symbol = WeatherClassInstance.Symbol;
        HourForecastTime = WeatherClassInstance.HourForecastTime;
        HourForecastTemperature = WeatherClassInstance.HourForecastTemperature;
        HourForecastPng = WeatherClassInstance.HourForecastPng;
        DayName = WeatherClassInstance.DayName;
        DayIcon = WeatherClassInstance.DayIcon;
        DayMin = WeatherClassInstance.DayMin;
        DayMax = WeatherClassInstance.DayMax;
    }
}