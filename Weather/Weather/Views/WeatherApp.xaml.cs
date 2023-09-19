using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.ViewModels;

namespace Weather.Views;

public partial class WeatherApp : UserControl
{
    public WeatherApp()
    {
        InitializeComponent();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Button clickedButton = (Button)sender;
        Button otherButton = clickedButton == CButton ? FButton : CButton;
        if (clickedButton.Style == FindResource("Button"))
        {
            clickedButton.Style = FindResource("Activebutton") as Style;
            otherButton.Style = FindResource("Button") as Style;
        }
    }
    private void TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(SearchWrite.Text) && SearchWrite.Text.Length > 0)
            SearchBox.Visibility = Visibility.Collapsed;
        else
            SearchBox.Visibility = Visibility.Visible;
    }
    private void SearchBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        SearchWrite.Focus();
    }
}