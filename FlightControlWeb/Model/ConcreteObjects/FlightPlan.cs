using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Model.ConcreteObjects
{
    /*
     * The class is representing one segment of FlightPlan
     */
    public class Segment
    {
        [JsonPropertyName("longitude")]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]
        [JsonProperty("latitude")]
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
        [JsonPropertyName("longitude")]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("date_time")]
        [JsonProperty("date_time")]
        [Required]
        public DateTime DateTime { get; set; }
    }

    /*
     * The class is representing one Flight Plan and its details.
     */
    public class FlightPlan
    {
        [JsonPropertyName("passengers")]
        [JsonProperty("passengers")]
        public int Passengers { get; set; }
        [JsonPropertyName("company_name")]
        [JsonProperty("company_name")]
        [Required]
        public string CompanyName { get; set; }
        [JsonPropertyName("initial_location")]
        [JsonProperty("initial_location")]
        [Required]
        public InitialLocation InitialLocation { get; set; }
        [JsonPropertyName("segments")]
        [JsonProperty("segments")]
        [Required]
        public IEnumerable<Segment> Segments { get; set; }
    }
}