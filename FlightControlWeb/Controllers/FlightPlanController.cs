using System;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightPlanController : ControllerBase
    {
        private IFlightPlanManager flihtPlansModel = new FlightPlanManager();
        private IFlightManager flightsModel = new FlightManager();

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public FlightPlan GetFlightPlan(string id)
        {
            return this.flihtPlansModel.getFlightPlan(id);
        }

        private string generateFlight_Id(string companyName)
        {
            bool isGoodKey = true;
            string flight_id = "";

            // creating the special id for both Flight and FlighPlan
            while (isGoodKey == true)
            {
                flight_id += companyName;

                Random r = new Random();
                int number = r.Next(1, 1000);
                flight_id += number.ToString();

                if (this.flihtPlansModel.isIdExist(flight_id) == 0)
                {
                    // the key does not exist - the key is good
                    isGoodKey = false;
                }
                else
                    flight_id = "";
            }

            return flight_id;
        }

        // POST: api/FlightPlan
        [HttpPost]
        public void AddNewFlightPlan(FlightPlan newFlightPlan)
        {
            string flight_id = this.generateFlight_Id(newFlightPlan.Company_Name);

            // creating the flight object from the flight plan
            Flight newFlight = new Flight();
            newFlight.Flight_ID = flight_id;
            newFlight.Longitude = newFlightPlan.Initial_Location.Longitude;
            newFlight.Latitude = newFlightPlan.Initial_Location.Latitude;
            newFlight.Passengers = newFlightPlan.Passengers;
            newFlight.Company_Name = newFlightPlan.Company_Name;
            newFlight.Date_Time = newFlightPlan.Initial_Location.Date_Time;
            newFlight.Is_External = false; ///////////////////////////////////////////true or flase?/////////////////

            // adding the id also for the fligh plan
            newFlightPlan.Flight_ID = flight_id;

            // adding the new flight to the list
            this.flightsModel.addNewFlight(newFlight);

            // adding the flight plan
            this.flihtPlansModel.addNewFlightPlan(newFlightPlan);
        }
    }
}