using System.ComponentModel.DataAnnotations;

namespace ApiTodo.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
