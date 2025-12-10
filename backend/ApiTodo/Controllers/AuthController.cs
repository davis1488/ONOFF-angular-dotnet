using ApiTodo.Data;
using ApiTodo.DTOs;
using ApiTodo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly AppDbContext _context;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, AppDbContext context)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            
            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var token = _authService.GenerateToken(user);
            
            return Ok(new 
            { 
                token,
                user = new 
                {
                    id = user.Id,
                    email = user.Email
                }
            });
        }

        // TEMPORAL: Endpoint para regenerar el hash de password
        [HttpPost("fix-password")]
        public async Task<IActionResult> FixPassword()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == "user@test.com");
            
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // Regenerar hash correcto para Password123!
            string newHash = AuthService.HashPassword("Password123!");
            user.PasswordHash = newHash;
            
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Password actualizado para: {Email}", user.Email);
            
            return Ok(new 
            { 
                message = "Password actualizado correctamente",
                newHash = newHash
            });
        }
    }
}
