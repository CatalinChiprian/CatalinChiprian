using System.Windows.Input;
using Weather.Models;

namespace Weather.Commands;

public class CelsiusCommand : CommandBase
{
    private readonly WeatherClass _weatherClass;
    private ICommand _enterCommand;

    public CelsiusCommand(WeatherClass weatherClass, ICommand enterCommand)
    {
        _weatherClass = weatherClass;
        _enterCommand = enterCommand;
    }

    public override void Execute(object parameter)
    {
        if (_weatherClass.Symbol != "c")
        {
            _weatherClass.Symbol = "c";
            _enterCommand.Execute(_weatherClass);
        }
    }
}