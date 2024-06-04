using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KinoUG.Server.Models
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    
        public List <Ticket> UserTickets { get; set; }
        


    }
}
