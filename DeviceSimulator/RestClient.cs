using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        private string JsonString;
        private HttpClient hclient;
        public RestClient(string _JsonString)
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.POST;
            JsonString = _JsonString;
            hclient = new HttpClient();
            hclient.BaseAddress = new Uri("http://192.168.43.62:5000/");
            hclient.DefaultRequestHeaders.Accept.Clear();
            hclient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }
        public void makeRequest()
        {
            PostAsync();
        }
        public async Task<float[]> PostAsync()
        {
            try
            {
                var content = new StringContent(JsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await hclient.PostAsync("api/metric", content);
                if (response.IsSuccessStatusCode)
                {
                    String res = await response.Content.ReadAsStringAsync();//   ReadAsAsync<String>();
                    Console.WriteLine(res);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
