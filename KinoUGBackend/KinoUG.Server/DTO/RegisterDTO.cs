using System.ComponentModel.DataAnnotations;

namespace KinoUG.Server.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        
        public string? UserName { get; set; }    

    }
}
