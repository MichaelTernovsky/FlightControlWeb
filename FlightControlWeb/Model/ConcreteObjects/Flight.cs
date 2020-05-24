using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one Flight and its details.
     */
    public class Flight
    {
        [JsonPropertyName("flight_id")]
        [JsonProperty("flight_id")]
        public string FlightID { get; set; }
        [JsonPropertyName("company_name")]
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
        [JsonPropertyName("is_external")]
        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }
        [JsonPropertyName("date_time")]
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }
    }
}