using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudie.Store;
using Cloudie.View;
using Cloudie.Model;
using Cloudie.ViewModel;

namespace CloudieTesting
{
    public class NavigationTest
    {
        private NavigationStore _navigationStore;
        private WeatherStats _weatherStats;

        public NavigationTest()
        {
            _navigationStore = new NavigationStore();
            _weatherStats = new WeatherStats();
        }

        [Fact]
        public void CurrentViewModel_WhenNew_ShouldBeNull()
        {
            Assert.Null(_navigationStore.CurrentViewModel);
        }

        [Fact]
        public void CurrentViewModel_WhenSet_ShouldUpdateCurrentViewModel()
        {
            var expected = new WeatherGraphViewModel(_navigationStore, _weatherStats);

            _navigationStore.CurrentViewModel = expected;

            Assert.Equal(expected, _navigationStore.CurrentViewModel);
        }
    }
}
