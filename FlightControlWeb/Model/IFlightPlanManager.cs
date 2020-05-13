using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The class is representing one Flights Plan and its details.
     */
    public interface IFlightPlanManager
    {
        void addNewFlightPlan(FlightPlan newFlightPlan);
        FlightPlan getFlightPlan(string flight_id);
        void deleteFlightPlan(string flight_id);
    }
}