using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private IFlightPlanManager model = new FlightPlanManager();

        // GET: api/FlightPlan
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public FlightPlan GetFlightPlan(string id)
        {
            return this.model.getFlightPlan(id);
        }

        // POST: api/FlightPlan
        [HttpPost]
        public void AddNewFlightPlan(FlightPlan newFlightPlan)
        {
            this.model.addNewFlightPlan(newFlightPlan);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteFlightPlan(string id)
        {
            this.model.deleteFlightPlan(id);
        }
    }
}