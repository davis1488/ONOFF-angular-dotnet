using ApiTodo.Models;

namespace ApiTodo.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        string GenerateToken(User user);
    }
}
