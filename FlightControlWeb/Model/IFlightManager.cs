using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The class is representing one Flights and its details.
     */
    public interface IFlightManager
    {
        IEnumerable<Flight> getAllInternalFlights();
        IEnumerable<Flight> getAllFlights(); // external and internal
    }
}