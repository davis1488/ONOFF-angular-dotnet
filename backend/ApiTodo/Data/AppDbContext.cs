using ApiTodo.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ApiTodo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relación User -> Todos
            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único para email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Usuario demo: user@test.com / Password123!
            // Hash precalculado para evitar problemas con migraciones
            var user = new User
            {
                Id = 1,
                Email = "user@test.com",
                PasswordHash = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYIBz9F.k6i" // Password123!
            };

            modelBuilder.Entity<User>().HasData(user);
        }
    }
}
