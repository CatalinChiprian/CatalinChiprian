using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Collections.ObjectModel;
using System.Reflection.PortableExecutable;
using System.Globalization;

namespace Cloudie.Model
{

    /// <summary>
    /// Represents the WeatherStats object that does all the work exctracting the data directly from the database.
    /// </summary>
    public class WeatherStats
    {
        private const string ConnectionString = "Data Source=weather-server1234.database.windows.net;Initial Catalog=Weather;Persist Security Info=True;User ID=cloudadmin;Password=adminCloud98*;Encrypt=True";

        /// <summary>
        /// Gets or sets the cities.
        /// </summary>
        public ObservableCollection<string> Cities { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the name of the gateway of the cities.
        /// </summary>
        public ObservableCollection<string> GateWayCities { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the gateways data.
        /// </summary>
        public Dictionary<string, WeatherData> GateAwaysData { get; set; } = new Dictionary<string, WeatherData>();

        /// <summary>
        /// Gets or sets the averaged data from the Data object.
        /// </summary>
        public ObservableCollection<WeatherData> AverageData { get; set; } = new ObservableCollection<WeatherData>();

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public IList<WeatherData> Data = new ObservableCollection<WeatherData>();
        public string? SelectedCity;
        private bool Outside = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherStats"/> class.
        /// </summary>
        public WeatherStats()
        {
            Task.Run(() => LoadData()).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Loads the data asynchronous.
        /// </summary>
        private async Task LoadData()
        {
            await CityLoad();
            await GateWayLoad();
        }

        /// <summary>
        /// Loads the names of the cities and their gateways.
        /// </summary>
        public async Task CityLoad()
        {
            try
            {
                string query = "SELECT * FROM DATA.Devices";
                var dt = await ExecuteQuery(query);

                Cities.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        Cities.Add(row["City"].ToString());
                        GateWayCities.Add(row["Gateway_ID"].ToString());
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Loads the gateway data.
        /// </summary>
        public async Task GateWayLoad()
        {
            try
            {
                string query = "SELECT * FROM DATA.Gateway";
                var dt = await ExecuteQuery(query);

                GateAwaysData.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        var name = new WeatherData
                        {
                            Gateway_ID = row["Gateway_ID"].ToString(),
                            Avg_Up_Time = Convert.ToDouble(row["Average_Up_Time"]),
                            Model_ID = row["MODEL_ID"].ToString(),
                            Lat = Convert.ToDouble(row["Lat"]),
                            Long = Convert.ToDouble(row["Long"]),
                            Uniq_ID = row["Uniq_ID"].ToString(),
                            Brand_ID = row["Brand_ID"].ToString(),
                            RSSI = row["rssi"].ToString(),
                            Battery_voltage = Convert.ToDouble(row["Battery_vol"]),
                            Altitude = row["Alt"] == DBNull.Value ? double.NaN : Convert.ToDouble(row["Alt"])
                        };
                        GateAwaysData.Add(row["Gateway_ID"].ToString(),name);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Loads the data for a specific city.
        /// </summary>
        /// <param name="City">The city.</param>
        public async Task DataLoad(string City)
        {
            if (City.EndsWith("-outside"))
            {
                City = City.Replace("-outside", "");
                Outside = true;
            }
            try
            {
                // Originally the query took the last 10 days of data, but the project is done and there is no new data coming in.
                // string query = "select * from DATA.Sensor as s join DATA.Devices as d on s.Device_ID = d.Device_ID WHERE s.Date_Time >= DATEADD(day,-10, GETDATE()) AND d.City = @City";

                string query = "select * from DATA.Sensor as s join DATA.Devices as d on s.Device_ID = d.Device_ID WHERE d.City = @City";
                var parameters = new List<SqlParameter> { new SqlParameter("@City", City) };
                var dt = await ExecuteQuery(query, parameters);

                AverageData.Clear();
                    Data.Clear();
                    dt.DefaultView.Sort = "Date_Time ASC";

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Ext_Sensor_Name"].ToString() == "Temperature Sensor" && Cities.IndexOf(row["Device_ID"].ToString() + "-outside") == -1)
                        {
                            Cities.Add(row["Device_ID"].ToString() + "-outside");
                            //Find index of Device_ID in cities and copy that index in Gateawaycities to the end of Gateawaycities
                            GateWayCities.Add(GateWayCities[Cities.IndexOf(row["Device_ID"].ToString())]);

                        }
                        var name = new WeatherData();

                        if (Outside)
                        {
                            name.Temperature = row["Ext_Sensor_Value"] == DBNull.Value ? double.NaN : Convert.ToDouble(row["Ext_Sensor_Value"]);
                            name.Ext_Sensor_Name = "Null";
                            name.Ext_Sensor_Value = double.NaN;
                        }
                        else
                        {
                            name.Temperature = row["Temperature"] == DBNull.Value ? double.NaN : Convert.ToDouble(row["Temperature"]);
                            name.Ext_Sensor_Name = row["Ext_Sensor_Name"] == DBNull.Value ? "Null" : row["Ext_Sensor_Name"].ToString();
                            name.Ext_Sensor_Value = name.Ext_Sensor_Name == "Temperature Sensor" ? double.NaN : (row["Ext_Sensor_Value"] == DBNull.Value ? double.NaN : Convert.ToDouble(row["Ext_Sensor_Value"]));
                        }

                        name.Humidity = row["Humidity"] == DBNull.Value ? double.NaN : Convert.ToDouble(row["Humidity"]);
                        name.Device_ID = row["Device_ID"].ToString();
                        name.Pressure = row["Pressure"] == DBNull.Value ? double.NaN : Convert.ToDouble(row["Pressure"]);
                        name.Date = Convert.ToDateTime(row["Date_Time"]);
                        Data.Add(name);
                    }

                    var a = Data.GroupBy(x => x.Date.Day)
                        .Where(group => group.Any(x => !double.IsNaN(x.Temperature)))
                        .Select(group =>
                        {
                            var temperatures = group.Where(x => !double.IsNaN(x.Temperature)).ToList();
                            return new
                            {
                                Day = group.Key,
                                AvgTemp = temperatures.Average(x => x.Temperature)
                            };
                        }).ToList();

                    foreach (var item in a)
                    {
                        var x = Data.Where(x => x.Date.Day == item.Day).FirstOrDefault();
                        x.AvgTemp = item.AvgTemp;
                        AverageData.Add(x);

                    }

                    Outside = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Executes the database connection and query execution.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data table.</returns>
        private async Task<DataTable> ExecuteQuery(string query, List<SqlParameter> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                SqlDataReader reader = await command.ExecuteReaderAsync();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }
    }
}
