using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Auth;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExists)
            {
                throw new Exception("Email is already registered.");
            }

            if (dto.Role != UserRoles.Customer &&
                dto.Role != UserRoles.Seller)
            {
                throw new Exception("Invalid user role.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email.ToLower(),
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = passwordHash,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully.";
        }
    }
}