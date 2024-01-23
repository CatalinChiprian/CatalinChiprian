using Cloudie.Model;

namespace CloudieTesting
{
    public class WeatherStatsTests
    {
        private WeatherStats _weatherStats;

        public WeatherStatsTests()
        {
            _weatherStats = new WeatherStats();
        }

        [Fact]
        public void Cities_WhenNew_ShouldNotBeNull()
        {
            Assert.NotNull(_weatherStats.Cities);
        }

        [Fact]
        public void GateWayCities_WhenNew_ShouldNotBeNull()
        {
            Assert.NotNull(_weatherStats.GateWayCities);
        }

        [Fact]
        public void GateAwaysData_WhenNew_ShouldNotBeNull()
        {
            Assert.NotNull(_weatherStats.GateAwaysData);
        }

        [Fact]
        public async Task CityLoad_ShouldLoadCities()
        {
            await _weatherStats.CityLoad();

            Assert.NotEmpty(_weatherStats.Cities);
        }

        [Fact]
        public async Task GateWayLoad_ShouldLoadGateways()
        {
            await _weatherStats.GateWayLoad();

            Assert.NotEmpty(_weatherStats.GateWayCities);
        }

        [Fact]
        public async Task DataLoad_ShouldLoadDataForCity()
        {
            string city = "lht-saxion";
            await _weatherStats.DataLoad(city);

            Assert.NotEmpty(_weatherStats.Data);
            Assert.NotEmpty(_weatherStats.AverageData);
        }
    }
}