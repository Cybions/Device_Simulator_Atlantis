using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceSimulator
{
    class Metric
    {
        public DateTime metricDate;
        public string name;
        public string macAdress;
        public Device.TypeOfDevice deviceType;
        public string metricValue;

        public Metric(string _name, string _macAdress, Device.TypeOfDevice _deviceType, string _metricValue)
        {
            metricDate = System.DateTime.Now;
            name = _name;
            macAdress = _macAdress;
            deviceType = _deviceType;
            metricValue = _metricValue;
        }
    }
}
