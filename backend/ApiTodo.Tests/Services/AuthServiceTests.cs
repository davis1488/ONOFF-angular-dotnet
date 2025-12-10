using ApiTodo.Services;
using ApiTodo.Models;
using ApiTodo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApiTodo.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AppDbContext _context;

        public AuthServiceTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            _mockConfig.Setup(x => x["Jwt:Key"]).Returns("SuperSecretKeyForJWTAuthenticationWith32Characters!!!");
            _mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            _context = new AppDbContext(options);
        }

        [Fact]
        public async Task ValidateUserAsync_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var service = new AuthService(_context, _mockConfig.Object, _mockLogger.Object);
            var password = "TestPassword123!";
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                PasswordHash = AuthService.HashPassword(password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await service.ValidateUserAsync("test@example.com", password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task ValidateUserAsync_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var service = new AuthService(_context, _mockConfig.Object, _mockLogger.Object);
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                PasswordHash = AuthService.HashPassword("CorrectPassword")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await service.ValidateUserAsync("test@example.com", "WrongPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateUserAsync_WithNonExistentUser_ReturnsNull()
        {
            // Arrange
            var service = new AuthService(_context, _mockConfig.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateUserAsync("nonexistent@example.com", "password");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GenerateToken_WithValidUser_ReturnsToken()
        {
            // Arrange
            var service = new AuthService(_context, _mockConfig.Object, _mockLogger.Object);
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                PasswordHash = "hash"
            };

            // Act
            var token = service.GenerateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            Assert.Contains(".", token); // JWT format
        }
    }
}
