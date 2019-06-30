using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeviceSimulator;
using System.Threading;
using System.Net.Http;

namespace DeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public Action<object> CreateSimulation = (object obj) => { DeviceSimulator.Program.StartSimulation(); } ;


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (!DeviceSimulator.Program.IsStarted)
            {
                Task startServer = new Task(CreateSimulation, "start");
                startServer.Start();
                return new string[] { "Server is starting. Please wait" };
            }
            return new string[] { "Server is Ready" }; 
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return DeviceSimulator.Program.GetDeviceStatus(id);
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] ValueItem value)
        {
            return DeviceSimulator.Program.ExecuteCMD(value.IPAdress, value.newStatus);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
