using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace FlightControlWeb.Model.Managers
{
    /*
     * The concrete flights plans manager of our web app
     */
    public class FlightPlanManager : IFlightPlanManager
    {
        private IMemoryCache cache;
        private IServerManager serverManager;

        public FlightPlanManager(IMemoryCache cache)
        {
            this.cache = cache;
            this.serverManager = new ServerManager(this.cache);
        }

        public void AddNewFlightPlan(FlightPlan newFlightPlan, string flightID)
        {
            // get the list from the cache
            var flightsDict = (Dictionary<string, FlightPlan>)cache.Get("flightsDict");

            flightsDict[flightID] = newFlightPlan;

            // insert the list to the cache
            cache.Set("flightsDict", flightsDict);
        }
        public void DeleteFlightPlan(string flight_id)
        {
            // get the list from the cache
            var flightsDict = (Dictionary<string, FlightPlan>)cache.Get("flightsDict");

            // check if the flight id exists
            bool isExist = flightsDict.ContainsKey(flight_id);

            // insert the list to the cache
            cache.Set("flightsDict", flightsDict);

            // delete the flight
            if (isExist)
                flightsDict.Remove(flight_id);
            else
                throw new Exception("Flight not found");
        }
        public async Task<FlightPlan> sendingRequest(HttpClient client, string serverURL, string id)
        {
            FlightPlan fp = null;
            try
            {
                // Geting the flight plan from the server.
                var resp = await client.GetStringAsync(serverURL + "/api/FlightPlan/" +
                    id.ToString());
                fp = JsonConvert.DeserializeObject<FlightPlan>(resp);
            }
            catch
            {
                throw new Exception("Could not get the data from the server");
            }

            return fp;
        }

        public async Task<FlightPlan> GetFlightPlan(string flight_id)
        {
            // get the list from the cache
            var flightsDict = (Dictionary<string, FlightPlan>)cache.Get("flightsDict");

            // the dictionary of servers
            var flightSource = (Dictionary<string, string>)cache.Get("flightSource");

            FlightPlan fp = null;

            // if it is internal flight plan
            if (flightsDict.ContainsKey(flight_id))
            {
                fp = flightsDict[flight_id];
            }
            else
            {
                if (flightSource.ContainsKey(flight_id))
                {
                    string serverURL = flightSource[flight_id];

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(serverURL);

                    client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                        ("application/json"));

                    // send request to get the flight plan
                    fp = await sendingRequest(client, serverURL, flight_id);
                }
                else
                {
                    throw new Exception("Flight not found");
                }
            }

            // insert the list to the cache
            cache.Set("flightSource", flightSource);

            // insert the list to the cache
            cache.Set("flightsDict", flightsDict);

            return fp;
        }
        public string GenerateFlightId(string companyName)
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

                if (this.IsIdExist(flight_id) == 0)
                {
                    // the key does not exist - the key is good
                    isGoodKey = false;
                }
                else
                    flight_id = "";
            }

            return flight_id;
        }
        public int IsIdExist(string flight_id)
        {
            // get the list from the cache
            var flightsDict = (Dictionary<string, FlightPlan>)cache.Get("flightsDict");

            // insert the list to the cache
            cache.Set("flightsDict", flightsDict);

            // delete the flight
            if (flightsDict.ContainsKey(flight_id))
                return 1;
            else
                return 0;
        }
    }
}