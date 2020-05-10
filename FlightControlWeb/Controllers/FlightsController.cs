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
    public class FlightsController : ControllerBase
    {
        private IFlightManager model = new FlightManager();

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights()
        {
            return this.model.getAllFlights();
        }


        // two get methods - how to use them ?


        /* 
        // GET: api/Flight
        [HttpGet]
        public IEnumerable<Flight> GetAllInternalFlights()
        {
            return this.model.getAllInternalFlights();
        }
        */
    }
}
