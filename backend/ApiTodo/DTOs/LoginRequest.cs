using System.ComponentModel.DataAnnotations;

namespace ApiTodo.DTOs
{
    public record LoginRequest(
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Required(ErrorMessage = "El email es requerido")]
        string Email,
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        string Password
    );
}
