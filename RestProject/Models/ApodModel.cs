using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RestProject.Models
{
    using SQLite;

    [Table("apod")]
    public class ApodModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonPropertyName("date")]
        [Indexed, NotNull]
        public string Date { get; set; }

        [JsonPropertyName("title")]
        [NotNull]
        public string TitleEnglish { get; set; }

        [JsonPropertyName("explanation")]
        [NotNull]
        public string ExplanationEnglish { get; set; }

        public string TitleCzech { get; set; }

        public string ExplanationCzech { get; set; }

        [JsonPropertyName("url")]
        [NotNull]
        public string Url { get; set; }

        [JsonPropertyName("hdurl")]
        public string HdUrl { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("service_version")]
        public string ServiceVersion { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}

