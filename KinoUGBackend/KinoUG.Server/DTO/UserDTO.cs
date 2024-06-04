namespace KinoUG.Server.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public List<TicketDTO> UserTickets { get; set; }
    }
}