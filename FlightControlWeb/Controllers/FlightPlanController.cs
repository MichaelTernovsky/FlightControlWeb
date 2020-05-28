﻿using System;
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
        public async Task<ActionResult<FlightPlan>> GetFlightPlan(string id)
        {
            try
            {
                return CreatedAtAction(actionName: "GetFlightPlan", await flightPlansModel.getFlightPlan(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/FlightPlan
        [HttpPost]
        public ActionResult AddNewFlightPlan(FlightPlan newFlightPlan)
        {
            // generate new id
            string flight_id = this.flightPlansModel.generateFlight_Id(newFlightPlan.CompanyName);

            // creating the flight object from the flight plan
            Flight newFlight = flightsModel.createFlightByFlightPlan(newFlightPlan, flight_id);
            newFlight.IsExternal = false;

            // adding the new flight to the list
            this.flightsModel.addNewFlight(newFlight);

            // adding the flight plan
            this.flightPlansModel.addNewFlightPlan(newFlightPlan, flight_id);

            return CreatedAtAction(actionName: "AddNewFlightPlan", newFlight);
        }
    }
}