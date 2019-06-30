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
    public class CommandController : ControllerBase
    {
        // GET api/Devices
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return DeviceSimulator.Program.GetAllDevices();
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return DeviceSimulator.Program.GetDeviceStatus(id);
        }


        // POST api/values
        [HttpPost]
        [Produces ("text/plain")]
        public string Post([FromBody] CommandItem cmdItem)
        {
            return DeviceSimulator.Program.ExecuteCMD(cmdItem.MACAdress, cmdItem.State);
        }
    }
}

