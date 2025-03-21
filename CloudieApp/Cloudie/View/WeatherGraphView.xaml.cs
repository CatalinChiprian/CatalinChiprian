using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cloudie.ViewModel;

namespace Cloudie.View
{
    /// <summary>
    /// Interaction logic for WeatherGraphView.xaml
    /// </summary>
    public partial class WeatherGraphView : UserControl
    {

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (WeatherGraphViewModel)DataContext;
            viewModel.WeatherTempVisible = true;
            viewModel.WeatherHumVisible = true;
            viewModel.WeatherIllVisible = true;
            viewModel.WeatherPressVisible = true;
        }
        private void CartesianChart_MouseLeftButtonDown_Temp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (WeatherGraphViewModel)DataContext;
            viewModel.WeatherTempVisible = true;
            viewModel.WeatherHumVisible = false;
            viewModel.WeatherIllVisible = false;
            viewModel.WeatherPressVisible = false;
        }

        private void CartesianChart_MouseLeftButtonDown_Hum(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (WeatherGraphViewModel)DataContext;
            viewModel.WeatherTempVisible = false;
            viewModel.WeatherHumVisible = true;
            viewModel.WeatherIllVisible = false;
            viewModel.WeatherPressVisible = false;
        }

        private void CartesianChart_MouseLeftButtonDown_Ill(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (WeatherGraphViewModel)DataContext;
            viewModel.WeatherTempVisible = false;
            viewModel.WeatherHumVisible = false;
            viewModel.WeatherIllVisible = true;
            viewModel.WeatherPressVisible = false;
        }

        private void CartesianChart_MouseLeftButtonDown_Press(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (WeatherGraphViewModel)DataContext;
            viewModel.WeatherTempVisible = false;
            viewModel.WeatherHumVisible = false;
            viewModel.WeatherIllVisible = false;
            viewModel.WeatherPressVisible = true;
        }

        private void UniformGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var viewModel = (WeatherGraphViewModel)DataContext;
            viewModel.ActualWidth = e.NewSize.Width;
            viewModel.ActualHeight = e.NewSize.Height;
        }
        public WeatherGraphView()
        {
            InitializeComponent();
        }

    }


    public class DivideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue && double.TryParse(parameter?.ToString(), out double divisor))
            {
                return doubleValue / divisor;
            }
            else if (value is int intValue && int.TryParse(parameter?.ToString(), out int intDivisor))
            {
                return intValue / intDivisor;
            }
            // You can add more checks or conversions for other types if needed

            throw new InvalidOperationException("Unsupported value type or invalid parameter type.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If you want to convert back (not used in this case)
            throw new NotImplementedException();
        }
    }
}
