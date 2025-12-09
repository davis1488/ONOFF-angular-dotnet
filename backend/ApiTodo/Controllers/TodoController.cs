using ApiTodo.Data;
using ApiTodo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ApiTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase


    // [ApiController]
    // [Route("api/[controller]")]
    // public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Todos.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return Ok(todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Todo todo)
        {
            var existing = await _context.Todos.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Titulo = todo.Titulo;
            existing.Completado = todo.Completado;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
