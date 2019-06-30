using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    public static class Program
    {
        private static int NumberOfDevice = 500; //Change this value for testing
        public static Device[] DeviceList;
        public static bool IsStarted = false;

        public static void Main(string[] args)
        {
            DeviceList = new Device[NumberOfDevice];
            StartSimulation();
            Console.WriteLine("Simulator is started");
        }

        private static Device[] GenerateDevices()
        {
            int i = 0;
            DeviceList = new Device[NumberOfDevice];
            while (i < NumberOfDevice)
            {
                DeviceList[i] = new Device(i);
                i++;
            }
            return DeviceList;
        }

        public static void StartSimulation()
        {
            DeviceList = GenerateDevices();
            Task[] TaskList = new Task[NumberOfDevice];
            int LoadedItems = 0;
            float LoadPercentage = 0;
            foreach(Device device in DeviceList)
            {
                TaskList[LoadedItems] = new Task(device.GenerateData,"test");
                LoadedItems++;
                float CurrentLoadPercentage = (float)LoadedItems / (float)NumberOfDevice;
                if (CurrentLoadPercentage != LoadPercentage)
                {
                    LoadPercentage = CurrentLoadPercentage;
                    Console.WriteLine("LOADING ----- " + LoadPercentage*100 + "%");
                }
            }
            Console.WriteLine("// \n // \n Nomber of device : " + LoadedItems + "\n // \n // \n");
            foreach(Task task in TaskList)
            {
                task.Start();
            }
            DisplayDevicesCategories();
            IsStarted = true;
            Console.ReadLine();
        }

        private static void DisplayDevicesCategories()
        {
            int Presence = 0, Temperature = 0, Light = 0, AtmosphericPressure = 0, Humidity = 0, 
                SoundLevel = 0, GPS = 0, CO2 = 0, LED = 0, Beeper = 0;
            foreach(Device device in DeviceList)
            {
                switch (device.GetTypeOfDevice())
                {
                    case Device.TypeOfDevice.Presence:
                        Presence++;
                        break;
                    case Device.TypeOfDevice.Temperature:
                        Temperature++;
                        break;
                    case Device.TypeOfDevice.Light:
                        Light++;
                        break;
                    case Device.TypeOfDevice.AtmosphericPressure:
                        AtmosphericPressure++;
                        break;
                    case Device.TypeOfDevice.Humidity:
                        Humidity++;
                        break;
                    case Device.TypeOfDevice.SoundLevel:
                        SoundLevel++;
                        break;
                    case Device.TypeOfDevice.GPS:
                        GPS++;
                        break;
                    case Device.TypeOfDevice.CO2:
                        CO2++;
                        break;
                    case Device.TypeOfDevice.LED:
                        LED++;
                        break;
                    case Device.TypeOfDevice.Beeper:
                        Beeper++;
                        break;
                }
            }
            Console.WriteLine("\n Presence: " + Presence);
            Console.WriteLine("Temperature: " + Temperature);
            Console.WriteLine("Light: " + Light);
            Console.WriteLine("AtmosphericPressure: " + AtmosphericPressure);
            Console.WriteLine("Humidity: " + Humidity);
            Console.WriteLine("SoundLevel: " + SoundLevel);
            Console.WriteLine("GPS: " + GPS);
            Console.WriteLine("CO2: " + CO2);
            Console.WriteLine("LED: " + LED);
            Console.WriteLine("Beeper: " + Beeper + "\n \n");

        }
        public static string ExecuteCMD(string MacAdress, bool newStatus)
        {
            foreach (Device device in DeviceList)
            {
                if (device.Device_MacAdress == MacAdress)
                {
                    string status = " ON";
                    if (!newStatus) { status = " OFF"; }
                    if (device.GetStatus() == newStatus) { return "This device is already" + status; }
                    device.status = newStatus;
                    return "{" + "\"success\": \"" + device.Device_Type.ToString().ToUpper() + " switched " + status + "\"" + "}";
                }
            }
            return "Device not found :( ";
        }

        public static string GetDeviceStatus(string MacAdress)
        {
            foreach(Device device in DeviceList)
            {
                if(device.Device_MacAdress == MacAdress)
                {
                    string state = "running";
                    if (!device.GetStatus()) { state = "shutted down"; }
                    return "The device at " + MacAdress + " is " + state;
                }
            }
            return "Device not found :( "; 
        }

        public static string[] GetAllDevices()
        {
            if (!IsStarted) { StartSimulation(); }
            string[] result = new string[NumberOfDevice];
            int i = 0;           
            foreach (Device device in DeviceList)
            {
                result[i] = device.Device_MacAdress + " : " + device.Device_Type;
                i++;
            }
            return result;
        }

        public static string GetServerStatus()
        {
            if (IsStarted)
            {
                return "Server Running";
            }
            return "Server Disconnected";
        }
    }
}
