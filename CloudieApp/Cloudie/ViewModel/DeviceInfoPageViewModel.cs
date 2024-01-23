using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cloudie.Store;
using Cloudie.Model;
using System.Windows.Input;
using Cloudie.Commands;
using System.Linq.Expressions;
using System.Data;
using System.Globalization;

namespace Cloudie.ViewModel
{
    /// <summary>
    /// Represents the device info page view model.
    /// </summary>
    public class DeviceInfoPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the weather stats.
        /// </summary>
        public WeatherStats WeatherS { get; }

        private string _location;
        private string _centerLocation;
        private short _zoomLevel;

        private string _city;

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
                //Cityindex is equal to the name of the city in the list of cities which is an observablecollection, Cityindex is used to get the gateaway id from GateWayCities, which will be used to get the data from GateAways
                var Cityindex = WeatherS.Cities.IndexOf(WeatherS.Cities.FirstOrDefault(x => x == City));
                var gtway = WeatherS.GateWayCities[Cityindex];
                Gateaway_ID = WeatherS.GateAwaysData.ContainsKey(gtway) ? WeatherS.GateAwaysData[gtway] : null;
                UpdateMap();

            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        /// <summary>
        /// Gets or sets the center location.
        /// </summary>
        public string CenterLocation
        {
            get { return _centerLocation; }
            set
            {
                _centerLocation = value;
                OnPropertyChanged(nameof(CenterLocation));
            }
        }

        /// <summary>
        /// Gets or sets the zoom level.
        /// </summary>
        public short ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                OnPropertyChanged(nameof(ZoomLevel));
            }
        }

        private WeatherData _gateaway_ID;

        /// <summary>
        /// Gets or sets the gateway ID.
        /// </summary>
        public WeatherData Gateaway_ID
        {
            get { return _gateaway_ID; }
            set
            {
                _gateaway_ID = value;
                OnPropertyChanged(nameof(Gateaway_ID));
            }
        }

        /// <summary>
        /// Gets the command to navigate to device info.
        /// </summary>
        public ICommand ToDeviceInfo { get; }

        /// <summary>
        /// Gets the command to navigate to graphs.
        /// </summary>
        public ICommand ToGraphs { get; }

        /// <summary>
        /// Gets the command to navigate to stats.
        /// </summary>
        public ICommand ToStats {  get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfoPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationStore">The navigation store.</param>
        /// <param name="weatherStats">The weather stats.</param>
        public DeviceInfoPageViewModel(NavigationStore navigationStore, WeatherStats weatherStats)
       {
            WeatherS = weatherStats;
            City = WeatherS.SelectedCity;
            ToGraphs = new ToGraphsCommand(navigationStore, weatherStats);
            ToStats = new ToStatsCommand(navigationStore, weatherStats);
            ToDeviceInfo = new ToDeviceInfoCommand(navigationStore, weatherStats);
       }

        /// <summary>
        /// Updates the map.
        /// </summary>
        private void UpdateMap()
        {
            Location = $"{Gateaway_ID.Lat.ToString(CultureInfo.InvariantCulture)},{Gateaway_ID.Long.ToString(CultureInfo.InvariantCulture)}";
            CenterLocation = Location;
            ZoomLevel = 12;
        }
    }
}
