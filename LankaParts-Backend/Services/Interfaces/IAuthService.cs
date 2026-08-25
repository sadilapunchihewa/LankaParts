using LankaParts_Backend.DTOs.Auth;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IAuthService
    {
        // Register a new customer or seller
        Task<string> RegisterAsync(RegisterDto dto);

        // Login and return user information with JWT token
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}