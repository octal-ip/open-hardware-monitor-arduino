using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using System.Diagnostics;
using System.Security.Principal;
using AsyncExtensions;
using Microsoft.Win32;


namespace ohm_arduino_gui
{
    public partial class MainForm : Form
    {
        private int _cpuCores = 0;
        private List<PortInfo> _ports = new List<PortInfo>();
        private readonly Computer _computer = new Computer();
        private readonly UpdaterVisitor _updater = new UpdaterVisitor();
        private readonly SensorVisitor _sensorVisitor;
        private readonly Dictionary<string, float> _sensorData = new Dictionary<string, float>();
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        ulong totalRAM;
        private bool isAdministrator;
        private string cpuTemp = "0";
        private string gpuTemp = "0";
        private string cpuUtil = "0";
        private string ramUtil = "0";
        private string[] outputList = new string[6];
        private int lastGradientValue;
        SerialPort comPort = null;

        private class PortInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        // Method to prepare the WMI query connection options.
        private static ConnectionOptions PrepareOptions()
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.Authentication = AuthenticationLevel.Default;
            options.EnablePrivileges = true;
            return options;
        }

        // Method to prepare WMI query management scope.
        private static ManagementScope PrepareScope(string machineName, ConnectionOptions options, string path)
        {
            ManagementScope scope = new ManagementScope();
            scope.Path = new ManagementPath(@"\\" + machineName + path);
            scope.Options = options;
            scope.Connect();
            return scope;
        }

        private static bool checkAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static List<PortInfo> FindComPorts()
        {
            List<PortInfo> portList = new List<PortInfo>();
            ConnectionOptions options = PrepareOptions();
            ManagementScope scope = PrepareScope(Environment.MachineName, options, @"\root\CIMV2");

            // Prepare the query and searcher objects.
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
            ManagementObjectSearcher portSearcher = new ManagementObjectSearcher(scope, objectQuery);

            using (portSearcher)
            {
                string caption = null;
                // Invoke the searcher and search through each management object for a COM port.
                foreach (ManagementObject currentObject in portSearcher.Get())
                {
                    if (currentObject != null)
                    {
                        object currentObjectCaption = currentObject["Caption"];
                        if (currentObjectCaption != null)
                        {
                            caption = currentObjectCaption.ToString();
                            if (caption.Contains("(COM"))
                            {
                                PortInfo portInfo = new PortInfo();
                                portInfo.Name = caption.Substring(caption.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty);
                                portInfo.Description = caption;
                                portList.Add(portInfo);
                            }
                        }
                    }
                }
            }
            return portList;
        }

        public MainForm()
        {
            _sensorVisitor = new SensorVisitor(ProcessSensor);

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OHMArduino");

            object regBaud = key.GetValue("Baud", null);
            object regInterval = key.GetValue("Interval", null);
            object regOutput = key.GetValue("Output", null);

            key.Close();

            InitializeComponent();
            if (Initialize())
            {
                mainTimer.Enabled = true;
                WindowState = FormWindowState.Minimized;
            }

            if (regBaud != null)
            {
                baudComboBox1.SelectedIndex = int.Parse(regBaud.ToString());
            }
            else
            {
                baudComboBox1.SelectedIndex = 0;
            }

            if (regInterval != null)
            {
                intervalComboBox1.SelectedIndex = int.Parse(regInterval.ToString());
            }
            else
            {
                intervalComboBox1.SelectedIndex = 1;
            }
            
            gradientTrackBar1.Enabled = false;

            if (checkAdministrator() == true)
            {
                adminLinkLabel1.Visible = false;
                outputList[0] = "Off";
                outputList[1] = "Manual";
                outputList[2] = "CPU Usage";
                outputList[3] = "RAM Usage";
                outputList[4] = "CPU Temp";
                outputList[5] = "GPU Temp";
                outputComboBox1.DataSource = outputList;
            }
            else
            {
                cpuTempLabel1.Visible = false;
                gpuTempLabel1.Visible = false;
                cpuTempBox.Visible = false;
                gpuTempBox.Visible = false;
                outputList[0] = "Off";
                outputList[1] = "Manual";
                outputList[2] = "CPU Usage";
                outputList[3] = "RAM Usage";
                outputComboBox1.DataSource = outputList;
            }

            if (regOutput != null)
            {
                outputComboBox1.SelectedIndex = int.Parse(regOutput.ToString());
            }
            else
            {
                outputComboBox1.SelectedIndex = 2;
            }
        }

        private void Log(string message)
        {
            if (logBox.Lines.Length > 15)
            {
                logBox.ReadOnly = false;
                logBox.Select(0, logBox.GetFirstCharIndexFromLine(logBox.Lines.Length - 15));
                logBox.SelectedText = "";
                logBox.ReadOnly = true;
            }
            logBox.AppendText(Environment.NewLine);
            message = $"[{DateTime.Now:HH:mm:ss}] {message}";
            logBox.AppendText(message);
        }

        private void Log(Exception ex, string title)
        {
            Log($"Exception {title} ({ex.GetType().FullName}): {ex.Message}");
        }

        private void intervalComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainTimer.Stop();
            mainTimer.Interval = int.Parse(intervalComboBox1.SelectedItem.ToString());
            mainTimer.Start();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OHMArduino");
            key.SetValue("Interval", int.Parse(intervalComboBox1.SelectedIndex.ToString()));  //Save the interval setting to the registry.
            key.Close();
        }


        private void ShowCPUTemp(string message)
        {
            cpuTempBox.Clear();
            cpuTempBox.AppendText(message + "°C");
        }

        private void ShowGPUTemp(string message)
        {
            gpuTempBox.Clear();
            gpuTempBox.AppendText(message + "°C");
        }

        private void ShowCPUUtil(string message)
        {
            cpuUtilBox.Clear();
            cpuUtilBox.AppendText(message + "%");
        }

        private void ShowRAMUtil(string message)
        {
            ramUtilBox.Clear();
            ramUtilBox.AppendText(message + "%");
        }

        private bool Initialize()
        {
            try
            {
                if (checkAdministrator() == true)
                {
                    isAdministrator = true;
                }
                else
                {
                    isAdministrator = false;
                }

                if (isAdministrator == true)  // Use Open Hardware Monitor if the process is started with administrative privileges.
                {
                    _computer.Open();
                    _computer.CPUEnabled = true;
                    _computer.GPUEnabled = true;
                    _computer.RAMEnabled = true;
                    Log("Running as admin: using Open Hardware Monitor");
                }
                else // Otherwise, just use the built-in Windows performance counters.
                {
                    cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                    totalRAM = (new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory)/1024/1024;
                    Log("Running without admin privileges: using Windows Performance Counters");
                }


                Log("Starting up...");

                var portNames = FindComPorts();

                _ports = portNames.ToList();

                foreach (var port in _ports)
                {
                    Log("Found " + port.Description);
                    comPortCombo.Items.Add($"{port.Description}");
                }

                if (comPortCombo.Items.Count > 0)
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OHMArduino");
                    object regPort = key.GetValue("Port", null); //Find which port was last selected.
                    key.Close();

                    int index = -1;

                    if (regPort != null)
                    {
                        index = _ports.FindIndex(p => p.Name.Contains(regPort.ToString())); //Use the last selected port if it's available.
                    }
                    if (index < 0)
                    {
                        index = _ports.FindIndex(p => p.Description.ToLower().Contains("ch340")); //Otherwise, try to find something that looks like an Arduino or STM32
                        if (index < 0)
                        {
                            index = _ports.FindIndex(p => p.Description.ToLower().Contains("usb serial device"));
                            if (index < 0)
                            {
                                throw new Exception("No Arduino port found.");
                            }
                        }
                    }

                    comPortCombo.SelectedIndex = index;
                    Log("Selecting COM port index: " + index);
                }
                else
                {
                    throw new Exception("No COM ports available.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Log(ex, "Error initializing.");
                return false;
            }
        }
        
        private void SendData(string data)
        {
            if (_ports.Count < 1 || comPortCombo.SelectedItem == null)
                return;

            try
            {
                int portBaudRate = int.Parse(baudComboBox1.SelectedItem.ToString());

                if (comPort.BaudRate != portBaudRate)
                {
                    comPort.Close();
                    comPort.BaudRate = portBaudRate;
                }
                if (comPort.IsOpen == false)
                {
                    comPort.Open();
                }

                comPort.Write(data);

                Log("Sent " + data + " over " + comPort.PortName + ". Length: " + data.Length);
            }
            catch (Exception ex)
            {
                Log(ex, "Error sending data.");
            }
        }

        private string GetSensorData(string key)
        {
            return _sensorData.ContainsKey(key) ? _sensorData[key].ToString("###") : "NA";
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (outputList[outputComboBox1.SelectedIndex] != "Off") //Only do something if the output is not "Off".
                {
                    if (outputList[outputComboBox1.SelectedIndex] == "Manual")
                    {
                        if (gradientTrackBar1.Value != lastGradientValue)
                        {
                            var serialStr = "<" + outputComboBox1.SelectedIndex + ",0,0,0,0," + gradientTrackBar1.Value + ">";
                            //Log("Sending sensor data:" + serialStr);
                            SendData(serialStr);
                            lastGradientValue = gradientTrackBar1.Value;
                        }
                    }
                    else
                    {
                        if (isAdministrator == true)
                        {
                            _sensorData.Clear();
                            _computer.Traverse(_updater);
                            _computer.Traverse(_sensorVisitor);

                            cpuTemp = GetSensorData("cpuTemp");
                            gpuTemp = GetSensorData("gpuTemp");
                            cpuUtil = GetSensorData("cpuUtil");
                            ramUtil = GetSensorData("ramUtil");
                        }
                        else
                        {
                            float cpuUsage = cpuCounter.NextValue();
                            cpuUtil = (String.Format("{0:0}", cpuUsage)); //Convert CPU usage to a string with no decimal places.
                            float ramUsage = ramCounter.NextValue();
                            ramUsage = ((totalRAM - ramUsage) / totalRAM) * 100;
                            ramUtil = (String.Format("{0:0}", ramUsage)); //Copnvert RAM usage to a string with no decimal places.
                        }

                        if (cpuUtil.Length == 0)
                        {
                            cpuUtil = "0";
                        }

                        var serialStr = "<" + outputComboBox1.SelectedIndex + "," + cpuUtil + "," + ramUtil + "," + cpuTemp + "," + gpuTemp + ",0>";
                        
                        //Log("Sending sensor data:" + serialStr);
                        ShowCPUTemp(cpuTemp);
                        ShowGPUTemp(gpuTemp);
                        ShowCPUUtil(cpuUtil);
                        ShowRAMUtil(ramUtil);
                        //gradientTrackBar1.Value = Int32.Parse(cpuUtil);
                        SendData(serialStr);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex, "Error updating sensor data.");
            }
        }

        private void ProcessSensor(ISensor sensor)
        {
            if (!sensor.Value.HasValue)
            {
                return;
            }
            if (sensor.SensorType != SensorType.Temperature)
            {
                if (sensor.SensorType != SensorType.Load)
                {
                    return;
                }
            }

            var value = sensor.Value.Value;

            if (sensor.Name.StartsWith("CPU Core #"))
            {
                if (Int32.TryParse(sensor.Name.Substring(10), out var id))
                {
                    _sensorData["cpuTemp" + sensor.Name.Substring(10)] = value;
                    if (id > _cpuCores)
                    {
                        _cpuCores = id;
                    }
                }

                return;
            }

            if (sensor.Name == "CPU Package")
            {
                if (sensor.SensorType == SensorType.Temperature)
                {
                    _sensorData["cpuTemp"] = value;
                }
            }

            if (sensor.Name == "GPU Core")
            {
                if (sensor.SensorType == SensorType.Temperature)
                {
                    _sensorData["gpuTemp"] = value;
                }
            }

            if (sensor.Name == "CPU Total")
            {
                if (sensor.SensorType == SensorType.Load)
                {
                    _sensorData["cpuUtil"] = value;
                }
            }

            if (sensor.Name == "Memory")
            {
                if (sensor.SensorType == SensorType.Load)
                {
                    _sensorData["ramUtil"] = value;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _computer.Close();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (!Visible)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void outputComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log("Mode: " + outputList[outputComboBox1.SelectedIndex]);

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OHMArduino"); //Save the output setting to the registry.
            key.SetValue("Output", int.Parse(outputComboBox1.SelectedIndex.ToString()));
            key.Close();

            if (outputList[outputComboBox1.SelectedIndex] == "Off")
            {
                var serialStr = "<" + outputComboBox1.SelectedIndex + ",0,0,0,0,0>";

                //Log("Sending disable data:" + serialStr);
                SendData(serialStr);

                if (comPort != null)
                {
                    comPort.Close(); //Close the COM port before switching.
                }

                ShowCPUTemp("-");
                ShowGPUTemp("-");
                ShowCPUUtil("-");
                ShowRAMUtil("-");
            }
            if (outputList[outputComboBox1.SelectedIndex] == "Manual")
            {
                gradientTrackBar1.Enabled = true;
                var serialStr = "<" + outputComboBox1.SelectedIndex + ",0,0,0,0," + gradientTrackBar1.Value + ">";

                //Log("Sending manual data:" + serialStr);
                SendData(serialStr);

                ShowCPUTemp("-");
                ShowGPUTemp("-");
                ShowCPUUtil("-");
                ShowRAMUtil("-");
            }
            else
            {
                gradientTrackBar1.Enabled = false;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
            startInfo.Verb = "runas";
            System.Diagnostics.Process.Start(startInfo);
            System.Windows.Forms.Application.Exit();
            return;
        }

        private void baudComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OHMArduino");  //Save the baud rate setting to the registry.
            key.SetValue("Baud", int.Parse(baudComboBox1.SelectedIndex.ToString()));
            key.Close();
        }

        private void comPortCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OHMArduino");
            key.SetValue("Port", _ports[comPortCombo.SelectedIndex].Name);  //Save the selected serial port to the registry.
            key.Close();

            if (comPort != null)
            {
                comPort.Close(); //Close the COM port before switching.
            }
            comPort = new SerialPort(_ports[comPortCombo.SelectedIndex].Name);
        }
    }
}
