using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestProject.Models
{
    using SQLite;

    [Table("apod")]
    public class ApodModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, NotNull]
        public string Date { get; set; }

        [NotNull]
        public string Title { get; set; }

        public string Explanation { get; set; }

        public string TitleCzech { get; set; }

        public string ExplanationCzech { get; set; }

        [NotNull]
        public string Url { get; set; }

        public string HdUrl { get; set; }

        public string MediaType { get; set; }

        public string Copyright { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}

