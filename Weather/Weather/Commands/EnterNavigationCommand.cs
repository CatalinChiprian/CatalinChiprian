using System;
using System.Threading.Tasks;
using System.Windows;
using Weather.Models;
using Weather.Store;
using Weather.ViewModels;

namespace Weather.Commands;

public class EnterNavigationCommand : CommandBase
{

    private readonly NavigationStore _navigationStore;
    private readonly WeatherClass _weatherClass;
    private readonly WeatherWelcomeViewModel _weatherWelcomeViewModel;

    public EnterNavigationCommand(NavigationStore navigationStore, WeatherClass weatherClass,
        WeatherWelcomeViewModel weatherWelcomeViewModel)
    {
        _navigationStore = navigationStore;
        _weatherClass = weatherClass;
        _weatherWelcomeViewModel = weatherWelcomeViewModel;
    }

    public override async void Execute(object parameter)
    {
        try
        {
            await Search();
            _navigationStore.CurrentViewModel = new WeatherViewModel(_navigationStore, _weatherClass);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Please try again!", MessageBoxButton.OK, MessageBoxImage.Information);
            _weatherWelcomeViewModel.CitySearch = "";
        }
    }


    private async Task Search()
    {
        await _weatherClass.Setup(_weatherWelcomeViewModel.CitySearch);
        _weatherClass.Forecast();
    }
}