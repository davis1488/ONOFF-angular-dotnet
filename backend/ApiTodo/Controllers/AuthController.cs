using ApiTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public record LoginRequest(string Email, string Password);

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
