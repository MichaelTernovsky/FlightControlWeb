using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServersController : ControllerBase
    {
        private IServerManager model = new ServerManager();

        // GET: api/Servers
        [HttpGet]
        public IEnumerable<Server> GetAllExternalServers()
        {
            return this.model.getAllExternalServers();
        }

        // GET: api/Servers/5
        [HttpGet("{serverID}", Name = "GetServer")]

        public Server GetServer(string serverID)
        {
            return this.model.getServer(serverID);
        }

        // POST: api/Servers
        [HttpPost]
        public void AddNewServer(Server newServer)
        {
            this.model.addNewServer(newServer);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{serverID}")]
        public void DeleteServer(string serverID)
        {
            this.model.deleteServer(serverID);
        }
    }
}