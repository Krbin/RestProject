using SQLite;

namespace BlazorRestProject.Models;

[Table("apod")]
public class ApodModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed, NotNull]
    public string Date { get; set; }
    [NotNull]
    public string TitleEnglish { get; set; }
    [NotNull]
    public string ExplanationEnglish { get; set; }
    public string TitleCzech { get; set; }
    public string ExplanationCzech { get; set; }
    [NotNull]
    public string Url { get; set; }
    public string HdUrl { get; set; }
    public string MediaType { get; set; }
    public string Copyright { get; set; }
    public string ServiceVersion { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public string Year { get; set; }
    public string Month { get; set; }
}

