using System;
using System.Collections.Generic;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one segment of FlightPlan
     */
    public class FlightSegment
    {
        // Properties
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double TimeSpan_Seconds { get; set; }
    }

    /*
     * The class is representing the inital location of FlightPlan
     */
    public class FlightInitialLocation
    {
        // Properties
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Date_Time { get; set; }
    }

    /*
     * The class is representing one Flight Plan and its details.
     */
    public class FlightPlan
    {
        // Properties
        public string Flight_ID { get; set; }
        public int Passengers { get; set; }
        public string Company_Name { get; set; }
        public FlightInitialLocation Initial_Location { get; set; }
        public IEnumerable<FlightSegment> Segments { get; set; }
    }
}