using System;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;
using Microsoft.Extensions.Caching.Memory;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightPlanController : ControllerBase
    {
        private IMemoryCache cache;
        private IFlightPlanManager flightPlansModel;
        private IFlightManager flightsModel;

        public FlightPlanController(IMemoryCache cache)
        {
            this.cache = cache;
            flightPlansModel = new FlightPlanManager(this.cache);
            flightsModel = new FlightManager(this.cache);
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public FlightPlan GetFlightPlan(string id)
        {
            return this.flightPlansModel.getFlightPlan(id);
        }

        // POST: api/FlightPlan
        [HttpPost]
        public void AddNewFlightPlan(FlightPlan newFlightPlan)
        {
            string flight_id = this.flightPlansModel.generateFlight_Id(newFlightPlan.CompanyName);

            // adding the id also for the fligh plan
            newFlightPlan.FlightID = flight_id;

            // creating the flight object from the flight plan
            Flight newFlight = flightsModel.createFlightByFlightPlan(newFlightPlan);
            newFlight.IsExternal = false;

            // adding the new flight to the list
            this.flightsModel.addNewFlight(newFlight);

            // adding the flight plan
            this.flightPlansModel.addNewFlightPlan(newFlightPlan);
        }
    }
}