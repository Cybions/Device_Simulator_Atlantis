using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceSimulator
{
    class JsonObject
    {

        public string GetJson(Metric metric)
        {
            string JsonString = string.Empty;
            JsonString =
                (
                "{" + 
                    "\"metricDate\": \"" + metric.metricDate + "\"," +
                    "\"deviceType\": \"" + metric.deviceType + "\"," +
                    "\"metricValue\": \"" + metric.metricValue + "\"" +
                "}"
                );
            return JsonString;
        }
    }
}
