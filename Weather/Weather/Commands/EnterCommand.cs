using System;
using System.Threading.Tasks;
using System.Windows;
using Weather.Models;
using Weather.ViewModels;

namespace Weather.Commands;

public class EnterCommand : CommandBase
{
    private readonly WeatherClass _weatherClass;

    private readonly WeatherViewModel _weatherViewModel;

    public EnterCommand(WeatherClass weatherClass, WeatherViewModel weatherViewModel)
    {
        _weatherClass = weatherClass;
        _weatherViewModel = weatherViewModel;
    }

    public override async void Execute(object parameter)
    {
        try
        {
            await Search();
            _weatherViewModel.PropertiesInit();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Please try again!", MessageBoxButton.OK, MessageBoxImage.Information);
            _weatherViewModel.CitySearch = _weatherClass.CitySearch;
        }
    }

    private async Task Search()
    {
        await _weatherClass.Setup(_weatherViewModel.CitySearch);
        _weatherClass.Forecast();
    }
}