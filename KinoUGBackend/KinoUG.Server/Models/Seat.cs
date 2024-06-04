namespace KinoUG.Server.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int HallId { get; set; } 
        public Hall Hall { get; set; } 
        public int SeatNumber { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
