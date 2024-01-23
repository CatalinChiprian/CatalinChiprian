using Cloudie.Model;
using Cloudie.Store;
using Cloudie.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cloudie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly WeatherStats _weatherStats;

        public App()
        {
            _weatherStats = new WeatherStats();
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new MainPageViewModel(_navigationStore, _weatherStats);
            MainWindow = new MainWindow()
            {
                DataContext = new MainView(_navigationStore)
            };

            MainWindow.Show();
        }
    }
}
