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
        IEnumerable<Flight> getAllInternalFlights(DateTime relative_to);
        Task<IEnumerable<Flight>> getAllFlights(DateTime relative_to);
        void addNewFlight(Flight newFlight);
        void deleteFlight(string flight_id);
        Flight createFlightByFlightPlan(FlightPlan fp);
        Flight isOccuringAtDateTime(FlightPlan fp, DateTime cuurentTime);
        Task<FlightPlan> getFlightPlans(List<Flight> flights, string url, HttpClient client);
    }
}