using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightControlWeb.Model.Managers
{
    /*
     * The concrete flights manager of our web app
     */
    public class FlightManager : IFlightManager
    {
        // the list of all the flights
        private static List<Flight> allFlightsList = new List<Flight>()
                    {
            new Flight{ Flight_ID="1",Longitude=1,Latitude=1,Passengers=1,Company_Name="1",Date_Time=new DateTime(),Is_External=false},
            new Flight{ Flight_ID="2",Longitude=2,Latitude=2,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            };

        public IEnumerable<Flight> getAllFlights()
        {
            return allFlightsList;
        }

        public IEnumerable<Flight> getAllInternalFlights()
        {
            List<Flight> internalFlights = new List<Flight>();

            foreach (Flight f in allFlightsList)
            {
                if (f.Is_External == false)
                    internalFlights.Add(f);
            }

            return internalFlights;
        }

        public void addNewFlight(Flight newFlight)
        {
            allFlightsList.Add(newFlight);
        }

        public void deleteFlightPlan(string flight_id)
        {
            Flight f = allFlightsList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();
            if (f != null)
                allFlightsList.Remove(f);
        }
    }
}