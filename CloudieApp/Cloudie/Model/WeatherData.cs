using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudie.Model
{
    /// <summary>
    /// Represents the weather data object that is used to store the data from the database.
    /// </summary>
    public class WeatherData
    {
        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets the humidity.
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// Gets or sets the external sensor name.
        /// </summary>
        public string Ext_Sensor_Name { get; set; }

        /// <summary>
        /// Gets or sets the external sensor value.
        /// </summary>
        public double Ext_Sensor_Value { get; set; }

        /// <summary>
        /// Gets or sets the pressure.
        /// </summary>
        public double Pressure { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the device ID.
        /// </summary>
        public string Device_ID { get; set; }

        /// <summary>
        /// Gets or sets the gateway ID.
        /// </summary>
        public string? Gateway_ID { get; set; }

        /// <summary>
        /// Gets or sets the average up time.
        /// </summary>
        public double? Avg_Up_Time { get; set; }

        /// <summary>
        /// Gets or sets the unique ID.
        /// </summary>
        public string? Uniq_ID { get; set; }

        /// <summary>
        /// Gets or sets the brand ID.
        /// </summary>
        public string? Brand_ID { get; set; }

        /// <summary>
        /// Gets or sets the RSSI.
        /// </summary>
        public string? RSSI { get; set; }

        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        public double? Altitude { get; set; }

        /// <summary>
        /// Gets or sets the model ID.
        /// </summary>
        public string? Model_ID { get; set; }

        /// <summary>
        /// Gets or sets the gateway name.
        /// </summary>
        public string? Gateway_name { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Long { get; set; }

        /// <summary>
        /// Gets or sets the battery voltage.
        /// </summary>
        public double? Battery_voltage { get; set; }

        /// <summary>
        /// Gets or sets the average temperature.
        /// </summary>
        public double AvgTemp { get; set; }
    }
}
