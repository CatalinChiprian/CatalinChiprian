using System.Windows.Input;
using Weather.Commands;
using Weather.Models;
using Weather.Store;

namespace Weather.ViewModels;

public class WeatherWelcomeViewModel : ViewModelBase
{
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

    public ICommand EnterCommand { get; }

    public WeatherWelcomeViewModel(NavigationStore navigationStore, WeatherClass weatherClass)
    {
        EnterCommand = new EnterNavigationCommand(navigationStore, weatherClass, this);
    }

}