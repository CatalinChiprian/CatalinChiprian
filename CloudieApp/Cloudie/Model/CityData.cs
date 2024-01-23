using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudie.Model
{
    /// <summary>
    /// Represents a city with its data that will be used for the graphs.
    /// </summary>
    public class CityData : ObservableObject
    {
        private string _name;

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private System.Windows.Media.Brush _cityColors;

        /// <summary>
        /// Gets or sets the color associated with the city.
        /// </summary>
        public System.Windows.Media.Brush CityColors
        {
            get { return _cityColors; }
            set { SetProperty(ref _cityColors, value); }
        }

        private bool _isChecked;

        /// <summary>
        /// Gets or sets a value indicating whether the city is checked.
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }
    }
}
