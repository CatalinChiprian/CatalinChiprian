using Cloudie.Model;
using Cloudie.Store;
using Cloudie.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudie.Commands
{

    /// <summary>
    /// Command for navigating to the MainPage.
    /// </summary>
    public class ToStatsCommand : CommandBase
    {
        /// <summary>
        /// The weather statistics.
        /// </summary>
        private readonly WeatherStats _weatherStats;

        /// <summary>
        /// The navigation store.
        /// </summary>
        private readonly NavigationStore _navigationStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToStatsCommand"/> class.
        /// </summary>
        /// <param name="navigationStore">The navigation store.</param>
        /// <param name="weatherStats">The weather statistics.</param>
        public ToStatsCommand(NavigationStore navigationStore, WeatherStats weatherStats)
        {
            _weatherStats = weatherStats;
            _navigationStore = navigationStore;
        }

        /// <summary>
        /// Executes the command with the specified parameter.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public override async void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel =
            new MainPageViewModel(_navigationStore, _weatherStats);
        }
    }
}
