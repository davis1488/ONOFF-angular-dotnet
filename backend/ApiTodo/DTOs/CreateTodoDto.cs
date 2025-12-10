using System.ComponentModel.DataAnnotations;

namespace ApiTodo.DTOs
{
    public record CreateTodoDto(
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
        string Titulo
    );
}
