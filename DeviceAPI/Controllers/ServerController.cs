using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeviceSimulator;
using System.Threading;

namespace DeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        public Action<object> CreateSimulation = (object obj) => { DeviceSimulator.Program.StartSimulation(); };


        // GET api/values
        [HttpGet]
        public string Get()
        {
            if (!DeviceSimulator.Program.IsStarted)
            {
                Task startServer = new Task(CreateSimulation, "start");
                startServer.Start();
                return "Server is starting. Please wait";
            }
            return "Server is Ready" ;
        }
    }
}
