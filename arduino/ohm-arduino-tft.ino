# Version 1.0 by Octal8
# https://hackaday.io/project/165236-windows-system-performance-display

#include <SPI.h>
#include "RunningAverage.h"
#include <Adafruit_ILI9341_STM.h> // STM32 DMA Hardware-specific library

#define TFT_CS  PB7
#define TFT_DC  PB9
#define TFT_RST PB8
#define ILI9341_VSCRDEF 0x33
#define ILI9341_VSCRSADD 0x37

Adafruit_ILI9341_STM tft = Adafruit_ILI9341_STM(TFT_CS, TFT_DC, TFT_RST);

#define TOP_FIXED_AREA 6
#define BOTTOM_FIXED_AREA 59

int scrollCount = TOP_FIXED_AREA;

const byte numChars = 32;
char receivedChars[numChars]; // an array to store the received data
char lastReceivedChars[numChars];
boolean newData = false;
char startMarker = '<';
char endMarker = '>';
char rc;
bool recvInProgress = false;
byte ndx = 0;

byte selectedOutput = 0;
byte cpuTemp = 0;
byte gpuTemp = 0;
byte cpuUtil = 0;
byte ramUtil = 0;
byte lastCpuTemp = 0;
byte lastGpuTemp = 0;
byte lastCpuUtil = 0;
byte lastRamUtil = 0;
byte lastMappedCpuTemp = 0;
byte lastMappedGpuTemp = 0;
byte lastMappedCpuUtil = 0;
byte lastMappedRamUtil = 0;

long lastNewData = 0;
byte manualValue = 0;

RunningAverage cpuUtilAv(20);
RunningAverage ramUtilAv(20);
RunningAverage cpuTempAv(20);
RunningAverage gpuTempAv(20);

int iterations = 0;

void setup() {
  Serial.begin(115200);

  tft.begin();
  tft.setRotation(1);
  tft.fillScreen(ILI9341_BLACK);

  tft.drawLine((320 - BOTTOM_FIXED_AREA + 5),0,(320 - BOTTOM_FIXED_AREA + 5),240,ILI9341_WHITE); // Draw the vertical line on the right Y axis.
  tft.drawLine((TOP_FIXED_AREA - 6),0,(TOP_FIXED_AREA - 6),240,ILI9341_WHITE); // Draw the vertical line on the left Y axis.
  tft.setTextColor(ILI9341_WHITE, ILI9341_BLACK);

  for (byte i = 0; i < 10; i++) {
      tft.drawLine((320 - BOTTOM_FIXED_AREA + 5),(i*24),(320 - BOTTOM_FIXED_AREA + 1),(i*24),ILI9341_WHITE); //Draw the Y-axis unit marks
      tft.drawLine((TOP_FIXED_AREA - 6),(i*24),(TOP_FIXED_AREA),(i*24),ILI9341_WHITE);
  }
  tft.drawLine((320 - BOTTOM_FIXED_AREA + 5),239,(320 - BOTTOM_FIXED_AREA + 1),239,ILI9341_WHITE); //Draw the Y-axis unit marks
  tft.drawLine((TOP_FIXED_AREA - 6),239,(TOP_FIXED_AREA),239,ILI9341_WHITE);
  setupScrollArea(TOP_FIXED_AREA, BOTTOM_FIXED_AREA); 
}


void setupScrollArea(uint16_t TFA, uint16_t BFA) {
  tft.writecommand(ILI9341_VSCRDEF); // Vertical scroll definition
  tft.spiwrite(TFA);
  tft.spiwrite(320 - TFA - BFA);
  tft.spiwrite(BFA);
}

void scrollAddress(uint16_t VSP) {
  tft.writecommand(ILI9341_VSCRSADD); // Vertical scrolling start address
  tft.spiwrite(VSP);
}

void loop() {
  recvWithStartEndMarkers();
  if (newData == true) {
    parseData();
    lastNewData = millis();
  }
  else if (millis() - lastNewData > 2500) { //Return to 0 if no data has been received for a while.
    cpuUtil = 0;
    ramUtil = 0;
    cpuTemp = 0;
    gpuTemp = 0;
  }
  showNewData();
  newData = false;
  iterations++;
}

void recvWithStartEndMarkers() {
    while (Serial.available() > 0 && newData == false) {
        rc = Serial.read();

        if (recvInProgress == true) {
            if (rc != endMarker) {
                receivedChars[ndx] = rc;
                ndx++;
                if (ndx >= numChars) {
                    ndx = numChars - 1;
                }
            }
            else {
                receivedChars[ndx] = '\0'; // terminate the string
                recvInProgress = false;
                ndx = 0;
                newData = true;
                while(Serial.available() > 0) {
                    Serial.read(); // Flush the serial buffer
                }
            }
        }

        else if (rc == startMarker) {
            recvInProgress = true;
        }
    }
}

void parseData() {
  // split the data into its parts
  char * strtokIndx; // this is used by strtok() as an index
  strtokIndx = strtok(receivedChars,",");  // get the first part - the string
  strcpy(receivedChars, strtokIndx); // copy it to strtokIndx

  selectedOutput = atoi(strtokIndx);     // convert to an integer
  strtokIndx = strtok(NULL, ",");
  cpuUtil = atoi(strtokIndx);     // convert to an integer
  strtokIndx = strtok(NULL, ",");
  ramUtil = atoi(strtokIndx);     // convert to an integer
  strtokIndx = strtok(NULL, ",");
  cpuTemp = atoi(strtokIndx);     // convert to an integer
  strtokIndx = strtok(NULL, ",");
  gpuTemp = atoi(strtokIndx);     // convert to an integer
  strtokIndx = strtok(NULL, ",");
  manualValue = atoi(strtokIndx); // convert to an integer
}

void showNewData() {
  cpuUtilAv.addValue(cpuUtil);
  ramUtilAv.addValue(ramUtil);
  cpuTempAv.addValue(cpuTemp);
  gpuTempAv.addValue(gpuTemp);

  float mappedcpuUtilAv = cpuUtilAv.getAverage()/100*239;
  float mappedramUtilAv = ramUtilAv.getAverage()/100*239;
  float mappedcpuTempAv = cpuTempAv.getAverage()/100*239;
  float mappedgpuTempAv = gpuTempAv.getAverage()/100*239;
  
  scrollAddress(scrollCount);

  tft.drawLine(scrollCount,240,scrollCount,0,ILI9341_BLACK);
  for (int i = 0; i < 11; i++) {
    tft.drawPixel(scrollCount, (i*24), 0x4A49); //Draw the very dark grey horizontal lines across the graph.
  }

  tft.drawLine(scrollCount, 239, scrollCount, abs(mappedcpuUtilAv-239), 0x5CBE);
  tft.drawPixel(scrollCount, abs(mappedcpuUtilAv-239), ILI9341_WHITE);
  
  tft.drawPixel(scrollCount, abs(mappedramUtilAv-239), ILI9341_GREEN);
  tft.drawPixel(scrollCount, abs(mappedramUtilAv-238), ILI9341_GREEN);

  if (mappedcpuTempAv > 0) {
    tft.drawPixel(scrollCount, abs(mappedcpuTempAv-239), ILI9341_RED);
    tft.drawPixel(scrollCount, abs(mappedcpuTempAv-238), ILI9341_RED);
  }

  if (mappedgpuTempAv > 0) {
    tft.drawPixel(scrollCount, abs(mappedgpuTempAv-239), ILI9341_YELLOW);
    tft.drawPixel(scrollCount, abs(mappedgpuTempAv-238), ILI9341_YELLOW);
  }

  if (scrollCount < 320 - BOTTOM_FIXED_AREA - 1) {
    scrollCount++;
  }
  else {
    scrollCount = TOP_FIXED_AREA;
  }
 
  if ((byte)mappedcpuUtilAv != (byte)lastMappedCpuUtil || iterations % 100 == 0) { //Only update if there's been a change, or every 100 iterations.
    tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),(byte)abs(lastMappedCpuUtil-231));
    tft.setTextColor(ILI9341_BLACK, ILI9341_BLACK);
    tft.print("CPU:"); tft.print(lastCpuUtil); tft.print("%");
    tft.setTextColor(0x5CBE, ILI9341_BLACK);
    lastMappedCpuUtil = (byte)mappedcpuUtilAv;
    lastCpuUtil = (byte)cpuUtilAv.getAverage();
    tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),(byte)abs(lastMappedCpuUtil-231));
    tft.print("CPU:"); tft.print(lastCpuUtil); tft.print("%");
  }

  if ((byte)mappedramUtilAv != (byte)lastMappedRamUtil || iterations % 100 == 0) { //Only update if there's been a change, or every 100 iterations.
    tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),abs(lastMappedRamUtil-231));
    tft.setTextColor(ILI9341_BLACK, ILI9341_BLACK);
    tft.print("RAM:"); tft.print(lastRamUtil); tft.print("%");
    tft.setTextColor(ILI9341_GREEN, ILI9341_BLACK);
    lastRamUtil = (byte)ramUtilAv.getAverage();
    lastMappedRamUtil = (byte)mappedramUtilAv;
    tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),abs(lastMappedRamUtil-231));
    tft.print("RAM:"); tft.print(lastRamUtil); tft.print("%");
  }

  if (mappedcpuTempAv > 0) {
    if ((byte)mappedcpuTempAv != (byte)lastMappedCpuTemp || iterations % 100 == 0) { //Only update if there's been a change, or every 100 iterations.
      tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),abs(lastMappedCpuTemp-231));
      tft.setTextColor(ILI9341_BLACK, ILI9341_BLACK);
      tft.print("CPU:"); tft.print(lastCpuTemp); tft.print((char)247);
      tft.setTextColor(ILI9341_RED, ILI9341_BLACK);
      lastCpuTemp = (byte)cpuTempAv.getAverage();
      lastMappedCpuTemp = (byte)mappedcpuTempAv;
      tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),abs(lastMappedCpuTemp-231));
      tft.print("CPU:"); tft.print(lastCpuTemp); tft.print((char)247);
    }
  }

  if (mappedgpuTempAv > 0) {
    if ((byte)mappedgpuTempAv != (byte)lastMappedGpuTemp || iterations % 100 == 0) { //Only update if there's been a change, or every 100 iterations.
      tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),abs(lastMappedGpuTemp-231));
      tft.setTextColor(ILI9341_BLACK, ILI9341_BLACK);
      tft.print("GPU:"); tft.print(lastGpuTemp); tft.print((char)247);
      tft.setTextColor(ILI9341_YELLOW, ILI9341_BLACK);
      lastGpuTemp = (byte)gpuTempAv.getAverage();
      lastMappedGpuTemp = (byte)mappedgpuTempAv;
      tft.setCursor((320 - BOTTOM_FIXED_AREA + 8),abs(lastMappedGpuTemp-231));
      tft.print("GPU:"); tft.print(lastGpuTemp); tft.print((char)247);
    }
  }
  delay(35);
}
