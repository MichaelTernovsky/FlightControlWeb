using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FlightControlWeb.Model.Managers
{
    /*
     * The concrete flights manager of our web app
     */
    public class FlightManager : IFlightManager
    {
        private IMemoryCache cache;
        private IServerManager serverModel;
        private IFlightPlanManager flightPlanModel;

        public FlightManager(IMemoryCache cache)
        {
            this.cache = cache;
            this.serverModel = new ServerManager(this.cache);
            this.flightPlanModel = new FlightPlanManager(this.cache);
        }

        public async Task<IEnumerable<Flight>> GetOuterFlights(Server server, DateTime relativeTo)
        {
            // creating the list of all flights
            List<Flight> serversFlightsList = new List<Flight>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(server.ServerURL);

            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                ("application/json"));
            List<Flight> outerFlights = null;

            try
            {
                var response = await client.GetStringAsync(server.ServerURL +
                    "/api/Flights?relative_to" +
                    "=" + relativeTo.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                outerFlights = JsonConvert.DeserializeObject<List<Flight>>(response);

                // adding the new flights to our list
                foreach (Flight flight in outerFlights)
                {
                    flight.IsExternal = true;
                    serversFlightsList.Add(flight);

                    // the dictionary of servers
                    var flightSource = (Dictionary<string, string>)cache.Get("flightSource");

                    // save the server and the flight
                    flightSource[flight.FlightID] = server.ServerURL;

                    // insert the list to the cache
                    cache.Set("flightSource", flightSource);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not get flights from the servers");
            }

            return serversFlightsList;
        }

        public async Task<IEnumerable<Flight>> GetAllFlights(DateTime relativeTo)
        {
            // get the server list from the server model
            List<Server> externalServers = (List<Server>)serverModel.GetAllExternalServers();
            // creating the list of all flights
            List<Flight> serversFlightsList = new List<Flight>();

            // get the flights list from each server in the servers list
            foreach (Server server in externalServers)
            {
                // get the flights from the server
                List<Flight> serverFlights = (List<Flight>)await
                    GetOuterFlights(server, relativeTo);

                // adding thos flights to our list
                serversFlightsList.AddRange(serverFlights);
            }

            // get all our internal flights
            List<Flight> internalFlightsList = (List<Flight>)await
                GetAllInternalFlights(relativeTo);

            // adding our internal flight to the list of all flights
            serversFlightsList.AddRange(internalFlightsList);

            return serversFlightsList;
        }

        public async Task<IEnumerable<Flight>> GetAllInternalFlights(DateTime relativeTo)
        {
            // get the list from the cache
            var allFlightsList = ((IEnumerable<Flight>)cache.Get("flights")).ToList();

            List<Flight> internalFlights = new List<Flight>();

            foreach (Flight f in allFlightsList)
            {
                FlightPlan fp = await flightPlanModel.GetFlightPlan(f.FlightID);

                if (f.IsExternal == false)
                {
                    // the flight is good for the current time
                    Flight newFlight = IsOccuringAtDateTime(fp, f.FlightID, relativeTo);
                    if (newFlight != null)
                        internalFlights.Add(newFlight);
                }
            }

            return internalFlights;
        }

        public Segment GetIsegment(FlightPlan fp, int i)
        {
            Segment indexSeg = null;
            int index = 0;

            foreach (Segment seg in fp.Segments)
            {
                if (index == i)
                    indexSeg = seg;

                index++;
            }

            return indexSeg;
        }
        public InitialLocation Interpolate(double frac, Segment A, Segment B)
        {
            InitialLocation cur = new InitialLocation();
            double diffXCordinate = B.Latitude - A.Latitude;
            double diffYCordinate = B.Longitude - A.Longitude;
            double lat = A.Latitude + diffXCordinate * frac;
            double lon = A.Longitude + diffYCordinate * frac;
            cur.Latitude = lat;
            cur.Longitude = lon;
            return cur;
        }
        public Flight IsOccuringAtDateTime(FlightPlan fp, string flightID, DateTime cuurentTime)
        {
            DateTime initialCpy = fp.InitialLocation.DateTime;
            TimeSpan diff = new TimeSpan();
            Segment A = new Segment();
            Segment B = new Segment();
            int i = 0;

            if (initialCpy >= cuurentTime)
                return null;

            while (initialCpy < cuurentTime)
            {
                diff = cuurentTime - initialCpy;

                Segment s = GetIsegment(fp, i);
                if (s != null)
                    initialCpy = initialCpy.AddSeconds(s.TimeSpanSeconds);
                else
                    return null;
                i++;
            }

            i--;

            // limit case
            if (i == 0)
            {
                A.Latitude = fp.InitialLocation.Latitude;
                A.Longitude = fp.InitialLocation.Longitude;
            }
            else
                A = GetIsegment(fp, i - 1);

            B = GetIsegment(fp, i);
            double frac = diff.TotalSeconds / B.TimeSpanSeconds;
            InitialLocation currLoc = Interpolate(frac, A, B);

            // creating the new flight
            Flight newFlight = CreateFlightByFlightPlan(fp, flightID);
            newFlight.Longitude = currLoc.Longitude;
            newFlight.Latitude = currLoc.Latitude;

            return newFlight;
        }
        public Flight CreateFlightByFlightPlan(FlightPlan fp, string flightID)
        {
            // create the corresponded flight
            Flight newFlight = new Flight
            {
                FlightID = flightID,
                Longitude = fp.InitialLocation.Latitude,
                Latitude = fp.InitialLocation.Longitude,
                Passengers = fp.Passengers,
                CompanyName = fp.CompanyName,
                DateTime = fp.InitialLocation.DateTime,
                IsExternal = false
            };

            return newFlight;
        }
        public void AddNewFlight(Flight newFlight)
        {
            // get the list from the cache
            var allFlightsList = ((IEnumerable<Flight>)cache.Get("flights")).ToList();

            allFlightsList.Add(newFlight);

            // insert the list to the cache
            cache.Set("flights", allFlightsList);
        }

        public void DeleteFlight(string flight_id)
        {
            // get the list from the cache
            var allFlightsList = ((IEnumerable<Flight>)cache.Get("flights")).ToList();

            Flight f = allFlightsList.Where(x => String.Equals(x.FlightID, flight_id))
                .FirstOrDefault();
            if (f != null)
            {
                allFlightsList.Remove(f);

                // insert the list to the cache
                cache.Set("flights", allFlightsList);
            }
            else
            {
                // insert the list to the cache
                cache.Set("flights", allFlightsList);

                throw new Exception("Flight was not found");
            }
        }
    }
}