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
        Task<IEnumerable<Flight>> GetAllInternalFlights(DateTime relativeTo);
        Task<IEnumerable<Flight>> GetAllFlights(DateTime relativeTo);
        Task<IEnumerable<Flight>> GetOuterFlights(Server server, DateTime relativeTo);
        void AddNewFlight(Flight newFlight);
        void DeleteFlight(string flight_id);
        Flight CreateFlightByFlightPlan(FlightPlan fp, string flightID);
        Flight IsOccuringAtDateTime(FlightPlan fp, string flightID, DateTime cuurentTime);
        Segment GetIsegment(FlightPlan fp, int i);
        InitialLocation Interpolate(double frac, Segment A, Segment B);
    }
}