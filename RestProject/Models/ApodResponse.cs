﻿using System.Text.Json.Serialization;

namespace BlazorRestProject.Models
{

    public class ApodResponse
    {

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("hdurl")]
        public string HdUrl { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("service_version")]
        public string ServiceVersion { get; set; }
    }
}

