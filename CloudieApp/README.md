# Cloudie

### Structure
- Structured the Application using the **MVVM** pattern.
- Using **UserControls** to separate the window attributes from the design elements.
- Structure is based on multiple grids, therefore naming is very important.
- Navigation Bar to all 3 pages.
- Responsive design, allowing for resizing of every UI element.

### Statistics Page
- Region (Sensor) selection bar.
- Statistics page: Current weather specificaitons.
- Weather history per days, with corresponding date and icon to reflect weather conditions.
- Average temperature Graph for individual region.

### Weather Graphs Page
- 4 Graphs representing: **temperature**, **illumination**, **pressure**, **humidity**.
- Possibility to choose which sensors to be displayed on the graphs.
- Graph selection in order to zoom in and compare values

### Sensor Info Page
- Information about individual sensors regarding characteristics of the device itself and the gateway.

**Dependecies:**
- MVVM Community Kit
- LiveChart.WPF
- SqlClient