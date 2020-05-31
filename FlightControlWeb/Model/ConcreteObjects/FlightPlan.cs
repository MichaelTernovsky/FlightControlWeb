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
        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; } = 200;

        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; } = 100;

        [JsonProperty("timespan_seconds")]
        [JsonPropertyName("timespan_seconds")]
        [Range(0, Double.MaxValue - 1)]
        public double TimeSpanSeconds { get; set; } = -1;
    }

    /*
     * The class is representing the inital location of FlightPlan
     */
    public class InitialLocation
    {
        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; } = 200;

        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; } = 100;

        [JsonProperty("date_time")]
        [JsonPropertyName("date_time")]
        [Range(typeof(DateTime), "0001-01-01T00:00:00Z", "9999-12-31T11:59:59Z")]
        public DateTime DateTime { get; set; }
    }

    /*
     * The class is representing one Flight Plan and its details.
     */
    public class FlightPlan
    {
        [JsonPropertyName("passengers")]
        [JsonProperty("passengers")]
        [Range(0, Int32.MaxValue - 1)]
        public int Passengers { get; set; } = -1;

        [JsonProperty("company_name")]
        [JsonPropertyName("company_name")]
        [Required]
        public string CompanyName { get; set; }

        [JsonProperty("initial_location")]
        [JsonPropertyName("initial_location")]
        [Required]
        public InitialLocation InitialLocation { get; set; }

        [JsonProperty("segments")]
        [JsonPropertyName("segments")]
        [Required]
        public IEnumerable<Segment> Segments { get; set; }
    }
}