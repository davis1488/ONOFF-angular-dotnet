using System.ComponentModel.DataAnnotations;

namespace ApiTodo.Models
{
    public class Todo
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;
        
        public bool Completado { get; set; }
        
        // Relación con usuario
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
