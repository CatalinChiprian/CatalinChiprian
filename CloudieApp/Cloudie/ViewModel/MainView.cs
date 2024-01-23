using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudie.Store;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cloudie.ViewModel
{
    /// <summary>
    /// Represents the main view model.
    /// </summary>
    public class MainView : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        /// <summary>
        /// Gets the current view model from the navigation store.
        /// </summary>
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        /// <param name="navigationStore">The navigation store.</param>
        public MainView(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        /// <summary>
        /// Handles the CurrentViewModelChanged event of the navigation store.
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}