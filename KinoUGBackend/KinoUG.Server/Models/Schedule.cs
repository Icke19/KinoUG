using System.ComponentModel.DataAnnotations.Schema;

namespace KinoUG.Server.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        [ForeignKey("HallId")]
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public List<Ticket> Tickets { get; set; }
        
    }
}
