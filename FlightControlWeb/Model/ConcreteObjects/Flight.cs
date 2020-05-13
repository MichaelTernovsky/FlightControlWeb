using System;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one Flight and its details.
     */
    public class Flight
    {
        // Properties
        public string Flight_ID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }

        public string Company_Name { get; set; }
        public DateTime Date_Time { get; set; }
        public bool Is_External { get; set; }
    }
}