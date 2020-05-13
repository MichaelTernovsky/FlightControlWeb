using System;
using System.Collections.Generic;
using System.Linq;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;

namespace FlightControlWeb.Model.Managers
{
    /*
     * The concrete flights plans manager of our web app
     */
    public class FlightPlanManager : IFlightPlanManager
    {
        // the list of all the flights
        private static List<FlightPlan> allFlightPlansList = new List<FlightPlan>()
                    {
            new FlightPlan{ Flight_ID="1",Company_Name="1",Initial_Location=new FlightInitialLocation{ Longitude=1,Latitude=1,Date_Time=new DateTime()},Segments=null },
            new FlightPlan{ Flight_ID="2",Company_Name="2",Initial_Location=new FlightInitialLocation{ Longitude=1,Latitude=2,Date_Time=new DateTime()},Segments=null },
            };

        public void addNewFlightPlan(FlightPlan newFlightPlan)
        {
            allFlightPlansList.Add(newFlightPlan);
        }

        public void deleteFlightPlan(string flight_id)
        {
            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();
            if (fp != null)
                allFlightPlansList.Remove(fp);
        }

        public FlightPlan getFlightPlan(string flight_id)
        {
            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();
            return fp;
        }

        // checking if the current id already exists in the list
        public int isIdExist(string flight_id)
        {
            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.Flight_ID, flight_id)).FirstOrDefault();

            if (fp == null)
                return 0;
            else
                return 1;
        }
    }
}