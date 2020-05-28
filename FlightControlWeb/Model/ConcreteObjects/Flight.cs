using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string FlightID { get; set; }
        [JsonPropertyName("company_name")]
        [JsonProperty("company_name")]
        [Required]
        public string CompanyName { get; set; }
        [JsonPropertyName("is_external")]
        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }
        [JsonPropertyName("date_time")]
        [JsonProperty("date_time")]
        [Required]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("longitude")]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("passengers")]
        [JsonProperty("passengers")]
        public int Passengers { get; set; }
    }
}