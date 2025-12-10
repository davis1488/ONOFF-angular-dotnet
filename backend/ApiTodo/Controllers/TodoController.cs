using ApiTodo.Data;
using ApiTodo.DTOs;
using ApiTodo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TodoController> _logger;

        public TodoController(AppDbContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? filter = "all")
        {
            var userId = GetUserId();
            _logger.LogInformation("Obteniendo tareas para usuario {UserId} con filtro {Filter}", userId, filter);

            var query = _context.Todos.Where(t => t.UserId == userId);

            query = filter?.ToLower() switch
            {
                "completed" => query.Where(t => t.Completado),
                "pending" => query.Where(t => !t.Completado),
                _ => query
            };

            var todos = await query
                .Select(t => new TodoDto(t.Id, t.Titulo, t.Completado, t.UserId))
                .ToListAsync();

            return Ok(todos);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = GetUserId();
            
            var total = await _context.Todos.CountAsync(t => t.UserId == userId);
            var completed = await _context.Todos.CountAsync(t => t.UserId == userId && t.Completado);
            var pending = await _context.Todos.CountAsync(t => t.UserId == userId && !t.Completado);

            return Ok(new 
            { 
                total, 
                completed, 
                pending 
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();
            
            var todo = new Todo
            {
                Titulo = dto.Titulo,
                Completado = false,
                UserId = userId
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea creada: {TodoId} por usuario {UserId}", todo.Id, userId);

            var todoDto = new TodoDto(todo.Id, todo.Titulo, todo.Completado, todo.UserId);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();
            var existing = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            
            if (existing == null)
            {
                return NotFound(new { message = "Tarea no encontrada" });
            }

            existing.Titulo = dto.Titulo;
            existing.Completado = dto.Completado;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea actualizada: {TodoId}", id);

            var todoDto = new TodoDto(existing.Id, existing.Titulo, existing.Completado, existing.UserId);
            return Ok(todoDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            
            if (todo == null)
            {
                return NotFound(new { message = "Tarea no encontrada" });
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea eliminada: {TodoId}", id);

            return Ok(new { message = "Tarea eliminada exitosamente" });
        }
    }
}
