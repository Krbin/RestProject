using SQLite;
using System;

namespace RestProject.Models
{
    [Table("ApodEntries")]
    public class ApodEntry
    {
        [PrimaryKey]
        public string Date { get; set; }

        [Indexed]
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Url { get; set; }
        public string HdUrl { get; set; }
        public string MediaType { get; set; }
        public string Copyright { get; set; }
        public string ServiceVersion { get; set; }

        [Ignore]
        public int Year => !string.IsNullOrEmpty(Date) && DateTime.TryParse(Date, out var dt) ? dt.Year : 0;
        [Ignore]
        public int Month => !string.IsNullOrEmpty(Date) && DateTime.TryParse(Date, out var dt) ? dt.Month : 0;
        [Ignore]
        public bool IsImage => MediaType?.ToLowerInvariant() == "image";

        [Ignore]
        public string ThumbnailUrl => IsImage ? Url : string.Empty;
    }
}