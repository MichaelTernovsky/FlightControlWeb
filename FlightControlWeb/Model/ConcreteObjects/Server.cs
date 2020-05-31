using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one Server and its details.
     */
    public class Server
    {
        [JsonPropertyName("ServerId")]
        [JsonProperty("ServerId")]
        [Required]
        public string ServerID { get; set; }
        [JsonProperty("ServerURL")]
        [JsonPropertyName("ServerURL")]
        [Required]
        public string ServerURL { get; set; }
    }
}