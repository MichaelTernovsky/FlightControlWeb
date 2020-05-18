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
            new Flight{ Flight_ID="3",Longitude=30,Latitude=300,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="4",Longitude=4,Latitude=44,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="5",Longitude=5,Latitude=554,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="6",Longitude=254,Latitude=265,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="7",Longitude=223,Latitude=225,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="8",Longitude=356,Latitude=765,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="9",Longitude=100,Latitude=100,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="10",Longitude=200,Latitude=200,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="11",Longitude=167,Latitude=237,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="12",Longitude=900,Latitude=700,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="13",Longitude=578,Latitude=500,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="14",Longitude=897,Latitude=798,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="15",Longitude=70,Latitude=10,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
            new Flight{ Flight_ID="16",Longitude=124,Latitude=984,Passengers=1,Company_Name="2",Date_Time=new DateTime(),Is_External=true},
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