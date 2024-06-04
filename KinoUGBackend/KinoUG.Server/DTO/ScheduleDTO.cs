using KinoUG.Server.Models;

namespace KinoUG.Server.DTO
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public MovieDTO Movie { get; set; }
        public List <SeatDTO> Seats { get; set; }   
    }
}
