using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one segment of FlightPlan
     */
    public class Segment
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [JsonPropertyName("timeSpan_seconds")]
        [JsonProperty("timeSpan_seconds")]
        public double TimeSpanSeconds { get; set; }
    }

    /*
     * The class is representing the inital location of FlightPlan
     */
    public class InitialLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [JsonPropertyName("date_time")]
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
    }

    /*
     * The class is representing one Flight Plan and its details.
     */
    public class FlightPlan
    {
        [JsonPropertyName("flight_id")]
        [JsonProperty("flight_id")]
        public string FlightID { get; set; }
        public int Passengers { get; set; }
        [JsonPropertyName("company_name")]
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
        [JsonPropertyName("initial_location")]
        [JsonProperty("initial_location")]
        public InitialLocation InitialLocation { get; set; }
        public IEnumerable<Segment> Segments { get; set; }
    }
}