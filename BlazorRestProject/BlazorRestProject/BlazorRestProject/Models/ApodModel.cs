using SQLite;

namespace BlazorRestProject.Models
{

    [Table("apod")]
    public class ApodModel
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Indexed]
        [NotNull]
        [Column("date")]
        public string Date { get; set; }

        [NotNull]
        [Column ("title_english")]
        public string TitleEnglish { get; set; }

        [NotNull]
        [Column("explanation_english")]
        public string ExplanationEnglish { get; set; }

        [Column("title_czech")]
        public string TitleCzech { get; set; }

        [Column("explanation_czech")]
        public string ExplanationCzech { get; set; }

        [NotNull]
        [Column("url")]
        public string Url { get; set; }

        [Column("hd_url")]
        public string HdUrl { get; set; }

        [Column("media_type")]
        public string MediaType { get; set; }

        [Column("copyright")]
        public string Copyright { get; set; }

        [Column("service_version")]
        public string ServiceVersion { get; set; }

        [Column("date_added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [Column("year")]
        public string Year { get; set; }

        [Column("month")]
        public string Month { get; set; }

    }

}
