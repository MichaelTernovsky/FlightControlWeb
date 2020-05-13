using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Managers;

namespace FlightControlWeb.Model.Interfaces
{
    /*
     * The class is representing one Flights Plan and its details.
     */
    public interface IFlightPlanManager
    {
        void addNewFlightPlan(FlightPlan newFlightPlan);
        FlightPlan getFlightPlan(string flight_id);
        void deleteFlightPlan(string flight_id);
        int isIdExist(string flight_id);
    }
}