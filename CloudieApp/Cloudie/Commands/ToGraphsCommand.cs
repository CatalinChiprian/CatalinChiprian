using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cloudie.ViewModel;
using Cloudie.Model;
using Cloudie.Store;

namespace Cloudie.Commands
{
    /// <summary>
    /// Represents the command to navigate to graphs.
    /// </summary>
    public class ToGraphsCommand : CommandBase
    {
        private readonly WeatherStats _weatherStats;
        private readonly NavigationStore _navigationStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToGraphsCommand"/> class.
        /// </summary>
        /// <param name="navigationStore">The navigation store.</param>
        /// <param name="weatherStats">The weather stats.</param>
        public ToGraphsCommand(NavigationStore navigationStore, WeatherStats weatherStats)
        {
            _weatherStats = weatherStats;
            _navigationStore = navigationStore;
        }

        /// <summary>
        /// Executes the command to navigate to graphs.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public override async void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel =
            new WeatherGraphViewModel(_navigationStore, _weatherStats);
        }
    }
}
