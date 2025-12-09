using ApiTodo.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ApiTodo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
        // OJO: quitamos DbSet<User> Users

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuario demo: user@test.com / Password123!
            var user = new User
            {
                Id = 1,
                Email = "user@test.com",
                PasswordHash = HashPassword("Password123!")
            };

            modelBuilder.Entity<User>().HasData(user);
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
