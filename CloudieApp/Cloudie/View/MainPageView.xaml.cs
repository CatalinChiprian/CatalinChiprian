using Cloudie.ViewModel;
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

namespace Cloudie.View
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        public MainPageView()
        {
            InitializeComponent();
            //Get the scrollview object

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            horizontalScrollViewer.ScrollToHorizontalOffset(horizontalScrollViewer.ScrollableWidth);
        }
    }

}


