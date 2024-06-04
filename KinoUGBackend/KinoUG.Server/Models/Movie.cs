using System.Reflection.Metadata;

namespace KinoUG.Server.Models
{
    public class Movie
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<Schedule> Schedules { get; set; }
      
    }
}
