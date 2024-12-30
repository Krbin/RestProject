using SQLite;

namespace BlazorRestProject.Models
{
    public class TestModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}