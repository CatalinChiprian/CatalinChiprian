using Cloudie.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudie.Store
{
    /// <summary>
    /// Represents the navigation store.
    /// </summary>
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel;

        /// <summary>
        /// Gets or sets the current view model.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        /// <summary>
        /// Called when the current view model changes.
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        /// <summary>
        /// Occurs when the current view model changes.
        /// </summary>
        public event Action CurrentViewModelChanged;
    }
}
