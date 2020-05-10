using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The class is representing one Flight and its details.
     */
    public class Flight
    {
        // Properties
        public string FlightID { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public int Passangers { get; set; }
        public string CompanyName { get; set; }
        public string DateTime { get; set; }
        public bool IsExternal { get; set; }
    }
}