using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one Server and its details.
     */
    public class Server
    {
        [Required]
        public string ServerID { get; set; }
        [Required]
        public string ServerURL { get; set; }
    }
}