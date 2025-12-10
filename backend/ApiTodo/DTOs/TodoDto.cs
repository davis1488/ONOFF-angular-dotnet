namespace ApiTodo.DTOs
{
    public record TodoDto(
        int Id,
        string Titulo,
        bool Completado,
        int UserId
    );
}
