using System.Windows.Input;
using Weather.Models;

namespace Weather.Commands;

public class FahrenheitCommand : CommandBase
{
    private readonly WeatherClass _weatherClass;
    private readonly ICommand _enterCommand;

    public FahrenheitCommand(WeatherClass weatherClass, ICommand enterCommand)
    {
        _weatherClass = weatherClass;
        _enterCommand = enterCommand;
    }

    public override void Execute(object parameter)
    {
        if (_weatherClass.Symbol != "f")
        {
            _weatherClass.Symbol = "f";
            _enterCommand.Execute(_weatherClass);
        }
    }
}