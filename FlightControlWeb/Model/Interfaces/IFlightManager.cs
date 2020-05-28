using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Managers;

namespace FlightControlWeb.Model.Interfaces
{
    /*
     * The class is representing one Flights and its details.
     */
    public interface IFlightManager
    {
        Task<IEnumerable<Flight>> getAllInternalFlights(DateTime relativeTo);
        Task<IEnumerable<Flight>> getAllFlights(DateTime relativeTo);
        Task<IEnumerable<Flight>> getOuterFlights(Server server, DateTime relativeTo);
        void addNewFlight(Flight newFlight);
        void deleteFlight(string flight_id);
        Flight createFlightByFlightPlan(FlightPlan fp, string flightID);
        Flight isOccuringAtDateTime(FlightPlan fp, string flightID, DateTime cuurentTime);
        Segment getIsegment(FlightPlan fp, int i);
        InitialLocation interpolate(double frac, Segment A, Segment B);
    }
}