using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private IMemoryCache cache;
        private IFlightManager flightsModel;
        private IFlightPlanManager flihtPlansModel;

        public FlightsController(IMemoryCache cache)
        {
            this.cache = cache;
            flightsModel = new FlightManager(this.cache);
            flihtPlansModel = new FlightPlanManager(this.cache);
        }

        // GET: api/Flights?relative_to=<DATE_TIME> OR api/Flights?relative_to=<DATE_TIME>&sync_all
        [HttpGet]
        public async Task<IEnumerable<Flight>> GetAllFlights(DateTime relative_to)
        {
            string requestStr = Request.QueryString.Value;
            bool isExternal = requestStr.Contains("sync_all");

            // return the correct list according to the "sync_all"
            if (isExternal)
            {
                IEnumerable<Flight> flights = await this.flightsModel.getAllFlights(relative_to);
                return flights;
            }
            else
                return this.flightsModel.getAllInternalFlights(relative_to);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteFlightPlan(string id)
        {
            this.flightsModel.deleteFlight(id);
            this.flihtPlansModel.deleteFlightPlan(id);
        }
    }
}