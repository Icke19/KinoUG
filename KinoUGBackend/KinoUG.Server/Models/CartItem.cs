using System.ComponentModel.DataAnnotations.Schema;

namespace KinoUG.Server.Models
{
    public class CartItem
    {
        public int Id { get; set;}
        [ForeignKey("TicketId")]
        public int TicketId { get; set;}
        public double Price { get; set; }
        public Ticket Ticket { get; set; }
        public int Quantity { get; set; }
    }
}
