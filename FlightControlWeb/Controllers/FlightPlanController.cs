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
        private IFlightPlanManager flihtPlansModel;
        private IFlightManager flightsModel;

        public FlightPlanController(IMemoryCache cache)
        {
            this.cache = cache;
            flihtPlansModel = new FlightPlanManager(this.cache);
            flightsModel = new FlightManager(this.cache);
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public FlightPlan GetFlightPlan(string id)
        {
            return this.flihtPlansModel.getFlightPlan(id);
        }

        // POST: api/FlightPlan
        [HttpPost]
        public void AddNewFlightPlan(FlightPlan newFlightPlan)
        {
            string flight_id = this.flihtPlansModel.generateFlight_Id(newFlightPlan.Company_Name);

            // adding the id also for the fligh plan
            newFlightPlan.Flight_ID = flight_id;

            // creating the flight object from the flight plan
            Flight newFlight = flightsModel.createFlightByFlightPlan(newFlightPlan);
            newFlight.Is_External = false;

            // adding the new flight to the list
            this.flightsModel.addNewFlight(newFlight);

            // adding the flight plan
            this.flihtPlansModel.addNewFlightPlan(newFlightPlan);
        }
    }
}