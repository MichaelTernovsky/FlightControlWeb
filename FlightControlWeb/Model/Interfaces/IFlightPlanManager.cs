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
     * The class is representing one Flights Plan and its details.
     */
    public interface IFlightPlanManager
    {
        void addNewFlightPlan(FlightPlan newFlightPlan, string flightID);
        Task<FlightPlan> getFlightPlan(string flight_id);
        void deleteFlightPlan(string flight_id);
        string generateFlight_Id(string companyName);
        int isIdExist(string flight_id);
    }
}