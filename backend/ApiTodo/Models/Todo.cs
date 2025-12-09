namespace ApiTodo.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public bool Completado { get; set; }
    }
}
