using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The concrete flights manager of our web app
     */

    public class FlightManager : IFlightManager
    {
        // the list of all the flights
        private static List<Flight> allFlightsList = new List<Flight>()
                    {
            new Flight{ FlightID="1",Longtitude=1,Latitude=1,Passangers=1,CompanyName="1",DateTime="1",IsExternal=false},
            new Flight{ FlightID="2",Longtitude=2,Latitude=2,Passangers=1,CompanyName="2",DateTime="2",IsExternal=true},
            new Flight{ FlightID="3",Longtitude=3,Latitude=3,Passangers=3,CompanyName="3",DateTime="3",IsExternal=false},
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
                if (f.IsExternal == false)
                    internalFlights.Add(f);
            }

            return internalFlights;
        }
    }
}