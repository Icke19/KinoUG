using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KinoUG.Server.Models
{
    public class Cart
    {
        public int Id { get; set;}
        [ForeignKey("UserId")]
        [Required]
        public string UserId { get; set;}
        public List <CartItem> Items { get; set;}
        [NotMapped]
        public double TotalPrice { get; set;}
    }
}
