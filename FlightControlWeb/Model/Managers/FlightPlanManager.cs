using System;
using System.Collections.Generic;
using System.Linq;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FlightControlWeb.Model.Managers
{
    /*
     * The concrete flights plans manager of our web app
     */
    public class FlightPlanManager : IFlightPlanManager
    {
        private IMemoryCache cache;
        public FlightPlanManager(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void addNewFlightPlan(FlightPlan newFlightPlan)
        {
            // get the list from the cache
            var allFlightPlansList = ((IEnumerable<FlightPlan>)cache.Get("flightPlans")).ToList();

            allFlightPlansList.Add(newFlightPlan);

            // insert the list to the cache
            cache.Set("flightPlans", allFlightPlansList);
        }

        public void deleteFlightPlan(string flight_id)
        {
            // get the list from the cache
            var allFlightPlansList = ((IEnumerable<FlightPlan>)cache.Get("flightPlans")).ToList();

            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();
            if (fp != null)
                allFlightPlansList.Remove(fp);

            // insert the list to the cache
            cache.Set("flightPlans", allFlightPlansList);
        }

        public FlightPlan getFlightPlan(string flight_id)
        {
            // get the list from the cache
            var allFlightPlansList = ((IEnumerable<FlightPlan>)cache.Get("flightPlans")).ToList();

            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();

            // insert the list to the cache
            cache.Set("flightPlans", allFlightPlansList);

            return fp;
        }

        public string generateFlight_Id(string companyName)
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

                if (this.isIdExist(flight_id) == 0)
                {
                    // the key does not exist - the key is good
                    isGoodKey = false;
                }
                else
                    flight_id = "";
            }

            return flight_id;
        }
        public int isIdExist(string flight_id)
        {
            // get the list from the cache
            var allFlightPlansList = ((IEnumerable<FlightPlan>)cache.Get("flightPlans")).ToList();

            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();

            // insert the list to the cache
            cache.Set("flightPlans", allFlightPlansList);

            if (fp == null)
                return 0;
            else
                return 1;
        }
    }
}