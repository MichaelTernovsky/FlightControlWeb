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
    public class FlightsController : ControllerBase
    {
        private IFlightManager flightsModel = new FlightManager();
        private IFlightPlanManager flihtPlansModel = new FlightPlanManager();

        // GET: api/Flights?relative_to=<DATE_TIME> OR api/Flights?relative_to=<DATE_TIME>&sync_all
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights(DateTime relative_to)
        {
            string requestStr = Request.QueryString.Value;
            bool isExternal = requestStr.Contains("sync_all");

            // return the correct list according to the "sync_all"
            if (isExternal)
                return this.flightsModel.getAllFlights();
            else
                return this.flightsModel.getAllInternalFlights();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteFlightPlan(string id)
        {
            this.flightsModel.deleteFlightPlan(id);
            this.flihtPlansModel.deleteFlightPlan(id);
        }
    }
}