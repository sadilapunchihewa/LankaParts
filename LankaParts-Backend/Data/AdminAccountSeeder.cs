using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Data
{
    public static class AdminAccountSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            var email = configuration["Admin:Email"]?.Trim().ToLowerInvariant();
            var password = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return;

            if (password.Length < 12)
                throw new InvalidOperationException("Admin:Password must contain at least 12 characters.");

            await using var scope = services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (await context.Users.AnyAsync(u => u.Email == email))
                return;

            context.Users.Add(new User
            {
                FirstName = configuration["Admin:FirstName"]?.Trim() ?? "System",
                LastName = configuration["Admin:LastName"]?.Trim() ?? "Administrator",
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = UserRoles.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }
}
