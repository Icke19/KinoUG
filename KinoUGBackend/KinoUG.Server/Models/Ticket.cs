using System.ComponentModel.DataAnnotations.Schema;

namespace KinoUG.Server.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get;set; }
        public User User { get; set; }
        [ForeignKey("SeatId")]
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public double Price { get; set;}
        [ForeignKey("ScheduleId")]
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        
    }
}
