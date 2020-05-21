﻿using FlightControlWeb.Model.ConcreteObjects;
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
        public async Task<IEnumerable<Flight>> getAllFlights(DateTime relative_to)
        {
            // get the server list from the server model
            List<Server> externalServers = (List<Server>)serverModel.getAllExternalServers();
            // creating the list of all flights
            List<Flight> serversFlightsList = new List<Flight>();
            // get all our internal flights
            List<Flight> internalFlightsList = (List<Flight>)this.getAllInternalFlights(relative_to);

            // get the flights list from each server in the servers list
            foreach (Server server in externalServers)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(server.ServerURL);

                client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<Flight> outerFlights = null;

                try
                {
                    var response = await client.GetStringAsync(server.ServerURL + "/api/Flights?relative_to=" + relative_to.ToString("yyyy-MM-ddTHH:mm:ss"));
                    outerFlights = JsonConvert.DeserializeObject<List<Flight>>(response);

                    // adding the new flights to our list
                    foreach (Flight flight in outerFlights)
                    {
                        flight.Is_External = true;
                        serversFlightsList.Add(flight);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed in external flights get reponse");
                }
            }

            // adding our internal flight to the list of all flights
            serversFlightsList.AddRange(internalFlightsList);

            return serversFlightsList;
        }

        public IEnumerable<Flight> getAllInternalFlights(DateTime relative_to)
        {
            // get the list from the cache
            var allFlightsList = ((IEnumerable<Flight>)cache.Get("flights")).ToList();

            List<Flight> internalFlights = new List<Flight>();

            foreach (Flight f in allFlightsList)
            {
                FlightPlan fp = flightPlanModel.getFlightPlan(f.Flight_ID);

                if (f.Is_External == false)
                {
                    // the flight is good for the current time
                    Flight newFlight = isOccuringAtDateTime(fp, relative_to);
                    if (newFlight != null)
                        internalFlights.Add(newFlight);
                }
            }

            return internalFlights;
        }

        public Flight isOccuringAtDateTime(FlightPlan fp, DateTime cuurentTime)
        {
            DateTime initalTime = fp.Initial_Location.Date_Time;
            Flight newFlight = null;
            FlightSegment segCurr = null, segPrev = null;
            bool isGoodSegments = false;

            // the flight is already ended
            if (initalTime > cuurentTime)
                return null;

            // run over the segments
            foreach (FlightSegment segment in fp.Segments)
            {
                initalTime = initalTime.AddSeconds(segment.TimeSpan_Seconds);

                if (initalTime >= cuurentTime)
                {
                    segPrev = segCurr;
                    segCurr = segment;
                    isGoodSegments = true;
                    break;
                }
            }

            if (isGoodSegments == false)
            {
                Console.WriteLine("Error in the segments array");
                return null;
            }

            // currTime - inital time of the i segment
            TimeSpan passedTime = cuurentTime - (initalTime.AddSeconds(-1 * (segCurr.TimeSpan_Seconds)));
            double div = passedTime.Seconds / segCurr.TimeSpan_Seconds;

            double startLon, startLat, endLon, endLat;
            // limit case - segment i is the first segment, this the prev is the inital
            if (segPrev == null)
            {
                startLon = fp.Initial_Location.Longitude;
                startLat = fp.Initial_Location.Latitude;
                endLon = segCurr.Longitude;
                endLat = segCurr.Latitude;
            }
            else
            {
                startLon = segPrev.Longitude;
                startLat = segPrev.Latitude;
                endLon = segCurr.Longitude;
                endLat = segCurr.Latitude;
            }

            double pathLength = Math.Sqrt(Math.Pow((endLat - startLat), 2) + Math.Pow((endLon - startLon), 2));
            double newLon = fp.Initial_Location.Longitude + pathLength;
            double newLat = fp.Initial_Location.Latitude + pathLength;

            // creating the new flight
            newFlight = createFlightByFlightPlan(fp);
            newFlight.Longitude = newLon;
            newFlight.Latitude = newLat;

            return newFlight;
        }
        public Flight createFlightByFlightPlan(FlightPlan fp)
        {
            Flight newFlight = new Flight { Flight_ID = fp.Flight_ID, Longitude = fp.Initial_Location.Latitude, Latitude = fp.Initial_Location.Longitude, Passengers = fp.Passengers, Company_Name = fp.Company_Name, Date_Time = fp.Initial_Location.Date_Time, Is_External = false };

            return newFlight;
        }

        public void addNewFlight(Flight newFlight)
        {
            // get the list from the cache
            var allFlightsList = ((IEnumerable<Flight>)cache.Get("flights")).ToList();

            allFlightsList.Add(newFlight);

            // insert the list to the cache
            cache.Set("flights", allFlightsList);
        }

        public void deleteFlight(string flight_id)
        {
            // get the list from the cache
            var allFlightsList = ((IEnumerable<Flight>)cache.Get("flights")).ToList();

            Flight f = allFlightsList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();
            if (f != null)
                allFlightsList.Remove(f);

            // insert the list to the cache
            cache.Set("flights", allFlightsList);
        }
    }
}