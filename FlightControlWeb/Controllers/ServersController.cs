using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;
using Microsoft.Extensions.Caching.Memory;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServersController : ControllerBase
    {
        private IMemoryCache cache;
        private IServerManager serverManager;

        public ServersController(IMemoryCache cache)
        {
            this.cache = cache;
            serverManager = new ServerManager(this.cache);
        }

        // GET: api/Servers
        [HttpGet]
        public ActionResult<IEnumerable<Server>> GetAllExternalServers()
        {
            return CreatedAtAction(actionName: "GetAllExternalServers", this.serverManager.getAllExternalServers());
        }

        // POST: api/Servers
        [HttpPost]
        public ActionResult AddNewServer(Server newServer)
        {
            this.serverManager.addNewServer(newServer);
            return CreatedAtAction(actionName: "AddNewServer", newServer);
        }

        // GET: api/Servers/5
        [HttpGet("{serverID}", Name = "GetServer")]

        public ActionResult<Server> GetServer(string serverID)
        {
            try
            {
                Server s = this.serverManager.getServer(serverID);
                return CreatedAtAction(actionName: "GetServer", s);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{serverID}")]
        public ActionResult DeleteServer(string serverID)
        {
            try
            {
                this.serverManager.deleteServer(serverID);
                return CreatedAtAction(actionName: "DeleteServer", "Server with ID " + serverID + " deleted");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}