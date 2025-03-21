using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudie.Model
{/// <summary>
 /// Represents the weather history.
 /// </summary>
    public class WeatherHistory
    {
        /// <summary>
        /// Gets or sets the temperature string.
        /// </summary>
        public string TempString { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherHistory"/> class.
        /// </summary>
        /// <param name="temperature">The temperature.</param>
        /// <param name="imagePath">The image path.</param>
        /// <param name="date">The date.</param>
        public WeatherHistory(double temperature, string imagePath, DateTime date)
        {
            TempString = Math.Round(temperature, 1).ToString(CultureInfo.InvariantCulture) + "°C";
            ImagePath = imagePath;
            Date = date.ToString("dd/MM/yyyy");
        }
    }
}
