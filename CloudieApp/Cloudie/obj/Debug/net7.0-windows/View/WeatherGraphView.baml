   M S B A M L   `   `   `  ����B  >Cloudie, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Cloudie.ViewCloudie.View  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      5clr-namespace:Cloudie.ViewModelCloudie.ViewModel  5      !clr-namespace:CloudieCloudie  5	      #    Cloudie.View.WeatherGraphView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:Cloudie.View     5      Fgraph4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      5	viewmodelclr-namespace:Cloudie.ViewModel     5      )covertsclr-namespace:Cloudie     5	      -   5          >�Name$  WeatherControl��5      $	��600��5      $	��500��5       � 	Resources 5      "   Cloudie.View.DivideConverter   DivideConverter   BooleanToVisibilityConverter  GridLengthConverter��%W   &	        &	      &	 &       5   
   6    �� 5   
   6'    � 5   
   6   5      .��5      � -   5        $��PageContent��5      $��Center=�5      ��5      ��  �� ElementName$ WeatherControl�� �� Path$ ActualWidth� � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   .+�5   
   � -   5        $��GraphOptions��5      $��0��6#   $��Center=�5      $��Center=�5        � ColumnDefinitions 5!      �� 5"      $��* �6#    5$      �� 5%      $
��Auto �6    �� 5&      $
��Auto �6    .+�5)      �� -   5        $
��Back��5)      $
��Left=�5*        SwitchPageButton#��[ 5+      $��0��5,      $��0��6%   5-      �� 5.      $��0��6   $��0��6)   $��Center=�5/      .+�50      ��   ComboBoxFormat#��[ 52      $	��110��53      $��23��54      $��Verdana)�55      $��Right=�56      ��50      �� $ UserDateSelectionValues� �� SelectedItem 51      �� $ SelectedFilter� �� Mode$ TwoWay=�56      (  "System.Windows.Controls.DatePicker  58      -   5          Name$ StartDateTextBox��5;      $��Right=�58      $	��110��59      $��23��6)   
���   �	   SelectedDate	 5<      �� *	StartDate+y�5=      �� $ DateVisibility�  5>      -   5        $ EndDateTextBox��5A      $��Right=�5>      $	��110��5?      $��23��6)   
���   �	 5B      �� *	EndDate+y�5C      �� $ DateVisibility�5D      �� 5F      $��Select a city��6     
TextFormat#��[ 5G      $��1��5H      $��0��6(   $��2��65   $��30(�5I      $��Center=�5J      $	��Top=�5K      $��0,0,0,30q�5L      �� 5N      $��1��6:   ��6   �� *Cities+��5O      �� 5P      .R�5Q      �� 
 �� Orientation$
 
Horizontal=�6%   5R      ��5T      �� 5U      .R�5V      �� ��6#   �� *Name+ d� 	IsChecked 6<   �� *	IsChecked+$ TwoWay=���6i   �� *DataContext.CheckedCity+ �� RelativeSource ��  �� AncestorType ItemsControl��6�   �� 5W      5Y      5Z      A� 5\   
   -   5        $��Graphs��5\      $��1��5]      $��Center=�5^      $
��Auto��5a      ��5`      �� $ WeatherControl��$ ActualWidth���5c      �� 5d       �� Color$ #00bfff��6"    �� Opacity$	 0.1Q�62   .+�5h      � $��Center=�5i      $��Center=�5j      y�5h      �� *WeatherTempVisible+ �� 	Converter# [ .+�5k      �� #��[ 6   $��Temperature��5l      $��20(�5m      $��Center=�5n      #  LiveCharts.Wpf.CartesianChart  5o      -   5        &   LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$
 None=�5o   F   $��Center=�5p      $��Center=�5q      $��0��5u      $��0��6+      Series 5o   '   �� *WeatherTemp+��5r      �� *GraphHeight+��5s      �� *
GraphWidth+   AxisY 5w        LiveCharts.Wpf.Axis     MinValue$	 -10Q�6%      
Foreground
 �   �   LabelFormatter 6G   �� *
YFormatter+   	Separator 5x        LiveCharts.Wpf.Separator  5y   "   ${�False��62   5{         AxisX 5~        
 �   �   Labels 6%   �� *Labels+6T   5�      5�      � 5�      $��Center=�5�      $��Center=�5�      y�5�      �� *WeatherPressVisible+# [ .+�5�      �� #��[ 6   $��Pressure��5�      $��20(�5�      $��Center=�5�        5�      -	   5        $
 None=�5�   G   $��Center=�5�      $��Center=�5�       5�   '   �� *WeatherPress+��5�      �� *GraphHeight+��6;   �� *
GraphWidth+ 5�        $	 970Q�6%   
 �   � 6G   �� *
YFormatter+ 5�        5�   "   ${�False��62   5�       5�        
 �   � 6%   �� *Labels+6T   5�      5�      � 5�      $��Center=�5�      y�5�      �� *WeatherHumVisible+# [ .+�5�      �� #��[ 6   $��Humidity��5�      $��20(�5�      $��Center=�5�        5�      -
   5        $
 None=�5�   E   $��Center=�5�      $��Center=�5�       5�   '   �� *
WeatherHum+��5�      �� *GraphHeight+��67   �� *
GraphWidth+ 5�        $ 0Q�6%   
 �   � 6E   �� *
YFormatter+ 5�        5�   "   ${�False��62   5�       5�        
 �   � 6%   �� *Labels+6T   5�      5�      � 5�      $��Center=�5�      y�5�      �� *WeatherIllVisible+# [ .+�5�      �� #��[ 6   $��Illumination��5�      $��20(�5�      $��Center=�5�        5�      -   5        $
 None=�5�   E   $��Center=�5�      $��Center=�5�       5�   '   �� *
WeatherIll+��5�      �� *GraphHeight+��67   �� *
GraphWidth+ 5�        $	 0.0Q�6%   
 �   � 6G   �� *
YFormatter+ 5�        5�   "   ${�False��62   5�       5�        
 �   � 6%   �� *Labels+6T   5�      5�      5�      5�      5�      