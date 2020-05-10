using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The class is representing one segment of FlightPlan
     */
    public class FlightSegment
    {
        // Properties
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public string TimeSpan_Seconds { get; set; }
    }

    /*
     * The class is representing the inital location of FlightPlan
     */
    public class FlightInitialLocation
    {
        // Properties
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public string Date_Time { get; set; }
    }

    /*
     * The class is representing one Flight Plan and its details.
     */
    public class FlightPlan
    {
        // Properties
        public int Passangers { get; set; }
        public string CompanyName { get; set; }
        public FlightInitialLocation Initial_Location { get; set; }
        public IEnumerable<FlightSegment> Segments { get; set; }
    }
}