using Weather.Store;
using Weather.ViewModels;

namespace Weather.Commands;

public class MainMenuCommand : CommandBase
{
    private readonly NavigationStore _navigationStore;
    private readonly WeatherViewModel _weatherViewModel;

    public MainMenuCommand(NavigationStore navigationStore, WeatherViewModel weatherViewModel)
    {
        _navigationStore = navigationStore;
        _weatherViewModel = weatherViewModel;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel =
            new WeatherWelcomeViewModel(_navigationStore, _weatherViewModel.WeatherClassInstance);
    }
}