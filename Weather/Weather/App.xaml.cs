using System.Threading.Tasks;
using System.Windows;
using Weather.Models;
using Weather.Store;
using Weather.ViewModels;

namespace Weather
{
    public partial class App : Application
    {
        //Credits to youtuber SingletonSean for instructions on how to implement multiple pages.
        private readonly WeatherClass _weatherClass;
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _weatherClass = new WeatherClass();
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new WeatherWelcomeViewModel(_navigationStore, _weatherClass);
            MainWindow = new MainWindow()
            {
                DataContext = new MainView(_navigationStore)
            };

            MainWindow.Show();
        }
    }
}