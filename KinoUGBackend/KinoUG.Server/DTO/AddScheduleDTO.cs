using KinoUG.Server.Models;

namespace KinoUG.Server.DTO
{
    public class AddScheduleDTO
    {
        public int MovieId { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; }
    }
}
