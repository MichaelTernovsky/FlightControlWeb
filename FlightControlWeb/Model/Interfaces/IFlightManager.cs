using System;
using System.Collections.Generic;
using System.Linq;
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
        IEnumerable<Flight> getAllInternalFlights();
        IEnumerable<Flight> getAllFlights(); // external and internal
        void addNewFlight(Flight newFlight);
        void deleteFlightPlan(string flight_id);
    }
}