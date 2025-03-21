using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cloudie.Commands;
using Cloudie.Model;
using Cloudie.Store;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cloudie.ViewModel
{
    /// <summary>
    /// Represents the main window view model.
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel() { }

        /// <summary>
        /// Command to exit the application.
        /// </summary>
        [RelayCommand]
        private static void ExitApp() => App.Current.Shutdown();
    }
}