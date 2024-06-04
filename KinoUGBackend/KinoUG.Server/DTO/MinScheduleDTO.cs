using KinoUG.Server.Models;

namespace KinoUG.Server.DTO
{
    public class MinScheduleDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string MovieTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

    }
}
