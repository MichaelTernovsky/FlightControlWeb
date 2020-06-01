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
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights([FromQuery(Name = "relative_to")]  DateTime relativeTo)
        {
            string requestStr = Request.QueryString.Value;
            bool isExternal = requestStr.Contains("sync_all");

            // convert the time to utc
            //DateTime convertedTime = TimeZoneInfo.ConvertTimeToUtc(relativeTo);
            DateTime convertedTime = relativeTo;

            try
            {
                // return the correct list according to the "sync_all"
                if (isExternal)
                {
                    IEnumerable<Flight> flights = await this.flightsModel.GetAllFlights(convertedTime)
                        ;
                    return CreatedAtAction(actionName: "GetAllFlights", flights);
                }
                else
                    return CreatedAtAction(actionName: "GetAllFlights", await flightsModel.GetAllInternalFlights(convertedTime));
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteFlight(string id)
        {
            try
            {
                this.flightsModel.DeleteFlight(id);
                this.flihtPlansModel.DeleteFlightPlan(id);
                return CreatedAtAction(actionName: "DeleteFlight", "Flight with ID " + id + " deleted");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}