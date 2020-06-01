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
        void AddNewFlightPlan(FlightPlan newFlightPlan, string flightID);
        Task<FlightPlan> GetFlightPlan(string flight_id);
        void DeleteFlightPlan(string flight_id);
        string GenerateFlightId(string companyName);
        int IsIdExist(string flight_id);
    }
}