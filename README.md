## Open Hardware Monitor - Arduino

Read CPU and GPU temperature readings, then display them as a scrolling graph on an STM32 Arduino TFT display.

Full details and build instructions: https://hackaday.io/project/165236-windows-system-performance-display

The full build in action: https://cdn.hackaday.io/files/1652367069457024/graph.gif

-----

### Requirements

1. Visual Studio 2017 with latest update
2. Windows 10 with .Net Framework 4.6.2
3. STM32 "blue pill" with ILI9341 TFT display
4. Platform.io
5. Bodmer's TFT_eSPI library: https://github.com/Bodmer/TFT_eSPI

### Usage

1. Download the latest source code (i.e. master zip) of [Open Hardware Monitor](https://github.com/openhardwaremonitor/openhardwaremonitor) and open it with VS2017. Then,
change the target platform of `OpenHardwareMonitorLib` to `.Net Framework 4.6.2`. Build this project (there is no need to build its GUI).
2. Download this project.
3. Compile and run.

A pre-compiled version is availabe in the "binaries" folder.

### Credits

Temperature data obtained from [Open Hardware Monitor](https://github.com/openhardwaremonitor/openhardwaremonitor)'s `OpenHardwareMonitorLib`

Icons made by [Pixel Buddha](https://www.flaticon.com/authors/pixel-buddha) from [www.flaticon.com](https://www.flaticon.com/) 
is licensed by [CC 3.0 BY](http://creativecommons.org/licenses/by/3.0/)

Inspiration and original code forked from https://github.com/logchan/open-hardware-monitor-arduino
