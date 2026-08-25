using LankaParts_Backend.DTOs.Auth;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
    }
}