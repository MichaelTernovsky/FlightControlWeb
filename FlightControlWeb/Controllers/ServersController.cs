using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServersController : ControllerBase
    {
        private IServerManager serverManager = new ServerManager();

        // GET: api/Servers
        [HttpGet]
        public IEnumerable<Server> GetAllExternalServers()
        {
            return this.serverManager.getAllExternalServers();
        }

        // GET: api/Servers/5
        [HttpGet("{serverID}", Name = "GetServer")]

        public Server GetServer(string serverID)
        {
            return this.serverManager.getServer(serverID);
        }

        // POST: api/Servers
        [HttpPost]
        public void AddNewServer(Server newServer)
        {
            this.serverManager.addNewServer(newServer);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{serverID}")]
        public void DeleteServer(string serverID)
        {
            this.serverManager.deleteServer(serverID);
        }
    }
}