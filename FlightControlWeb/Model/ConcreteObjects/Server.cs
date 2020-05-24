using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one Server and its details.
     */
    public class Server
    {
        public string ServerID { get; set; }
        public string ServerURL { get; set; }
    }
}