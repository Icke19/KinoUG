namespace KinoUG.Server.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public int ScheduleId { get; set; }
        public double Price { get; set; }
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
    }
}
