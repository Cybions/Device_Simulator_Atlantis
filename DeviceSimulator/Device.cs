using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceSimulator
{

    public class Device
    {
        public string Device_Name;
        public string Device_MacAdress;
        public TypeOfDevice Device_Type;
        public bool status;
        public enum TypeOfDevice
        {
            Presence,
            Temperature,
            Light,
            AtmosphericPressure,
            Humidity,
            SoundLevel,
            GPS,
            CO2,
            LED,
            Beeper
        }

        public Action<object> GenerateData;

        public Device(int itemNumber)
        {
            RandomMacAdress();
            status = true;
            RandomDeviceType();
            Device_Name = Device_Type.ToString() + "_" + itemNumber;
        }

        public TypeOfDevice GetTypeOfDevice()
        {
            return Device_Type;
        }

        private void RandomMacAdress()
        {
            Device_MacAdress = RandomMacSyllab() + ":" + RandomMacSyllab() + ":" + RandomMacSyllab() + ":" + RandomMacSyllab() + ":" + RandomMacSyllab() + ":" + RandomMacSyllab();
        }

        private string RandomMacSyllab()
        {
            string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rand = new Random();
            return Alphabet[rand.Next(Alphabet.Length)] + rand.Next(0, 10).ToString();
        }

        private void RandomDeviceType()
        {
            Random rand = new Random();
            int typeRandomized = rand.Next(0, 10); //Depend of number of type of device
            switch (typeRandomized)
            {
                case 0:
                    Device_Type = TypeOfDevice.Presence;
                    PresenceData();
                    break;
                case 1:
                    Device_Type = TypeOfDevice.Temperature;
                    TemperatureData();
                    break;
                case 2:
                    Device_Type = TypeOfDevice.Light;
                    LightData();
                    break;
                case 3:
                    Device_Type = TypeOfDevice.AtmosphericPressure;
                    AtmosphericPressureData();
                    break;
                case 4:
                    Device_Type = TypeOfDevice.Humidity;
                    HumidityData();
                    break;
                case 5:
                    Device_Type = TypeOfDevice.SoundLevel;
                    SoundLevelData();
                    break;
                case 6:
                    Device_Type = TypeOfDevice.GPS;
                    GPSData();
                    break;
                case 7:
                    Device_Type = TypeOfDevice.CO2;
                    CO2Data();
                    break;
                case 8:
                    Device_Type = TypeOfDevice.LED;
                    GenerateData = (object obj) => { };
                    break;
                case 9:
                    Device_Type = TypeOfDevice.Beeper;
                    GenerateData = (object obj) => { };
                    break;
            }
        }

        private void PresenceData()
        {
            float variance = 0.1f; //Chance of detection
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(false, variance); Thread.Sleep(10000); } };
        }
        private void TemperatureData()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * 30); //The reference can be between 0 and 30
            float variance = 1.5f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private void LightData()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * (5000 - 2000) + 2000); //The reference can be between 2000 and 5000
            float variance = 300f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private void AtmosphericPressureData()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * (1030 - 1010) + 1010); //The reference can be between 1010 and 1030
            float variance = 15f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private void HumidityData()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * 100); //This value is a percentage
            float variance = .5f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private void SoundLevelData()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * (150 - 10) + 10); //This value is a percentage
            float variance = 10f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private void GPSData()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * 6.382); //6.382 = Mount Evrest
            float variance = .2f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private void CO2Data()
        {
            Random rand = new Random();
            float Reference = (float)(rand.NextDouble() * 6.382); //6.382 = Mount Evrest
            float variance = .2f; //This delimit the variance
            GenerateData = (object obj) => { Thread.Sleep(RandomInitialDelay()); while (true) { CreateMetric(true, variance, Reference); Thread.Sleep(10000); } };
        }

        private static bool BoolRandomValue(float ratio)
        {
            Random rand = new Random();
            return (rand.Next(100) == ratio * 100);
        }
        private static float FloatRandomValue(float min, float max)
        {
            Random rand = new Random();
            double RandomizedValue = rand.NextDouble() * (max - min) + min;
            return (float)RandomizedValue;
        }

        private int RandomInitialDelay()
        {
            Random rand = new Random();
            return (rand.Next(0, 1000));
        }

        private void CreateMetric(bool isFloatMetric, float Variance = 0, float Reference = 0)
        {
            if (!status) { return; }
            string Value;
            if (isFloatMetric)
            {
                Value = FloatRandomValue(Reference - Variance, Reference + Variance).ToString();
            }
            else
            {
                Value = BoolRandomValue(Variance).ToString();
            }
            Metric newMetric = new Metric(Device_Name, Device_MacAdress, Device_Type, Value);
            //PostMetric(newMetric);
            PostViaRabbitMQ(newMetric);
        }


        private static void PostMetric(Metric metric)
        {
            JsonObject JsonMetric = new JsonObject();
            RestClient rClient = new RestClient(JsonMetric.GetJson(metric));
            Console.WriteLine(JsonMetric.GetJson(metric));
            rClient.makeRequest();
        }

        private static void PostViaRabbitMQ(Metric metric)
        {
            JsonObject JsonMetric = new JsonObject();
            RestClient rClient = new RestClient(JsonMetric.GetJson(metric));
            //Console.WriteLine(JsonMetric.GetJson(metric));
            var factory = new ConnectionFactory() { HostName = "192.168.43.62", UserName = "thomas", Password = "root" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs",
                                        type: "topic");

                //var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                var routingKey = "atlantis.metric";
                //var message = (args.Length > 1)
                //              ? string.Join(" ", args.Skip(1).ToArray())
                //              : "Hello World!";
                var message = JsonMetric.GetJson(metric);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "topic_logs",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
            }
        }
        public bool GetStatus()
        {
            return status;
        }
    }
}
