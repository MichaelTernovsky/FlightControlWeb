using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The concrete flights plans manager of our web app
     */
    public class FlightPlanManager : IFlightPlanManager
    {
        // the list of all the flights
        private static List<FlightPlan> allFlightPlansList = new List<FlightPlan>()
                    {
            new FlightPlan{ FlightID="1",CompanyName="1",Initial_Location=new FlightInitialLocation{ Longtitude=1,Latitude=1,Date_Time="1"},Segments=null },
            new FlightPlan{ FlightID="2",CompanyName="2",Initial_Location=new FlightInitialLocation{ Longtitude=1,Latitude=2,Date_Time="2"},Segments=null },
            };

        public void addNewFlightPlan(FlightPlan newFlightPlan)
        {
            allFlightPlansList.Add(newFlightPlan);
        }

        public void deleteFlightPlan(string flight_id)
        {
            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.FlightID, flight_id)).FirstOrDefault();
            if (fp != null)
                allFlightPlansList.Remove(fp);
        }

        public FlightPlan getFlightPlan(string flight_id)
        {
            FlightPlan fp = allFlightPlansList.Where(x => String.Equals(x.FlightID, flight_id)).FirstOrDefault();
            return fp;
        }
    }
}