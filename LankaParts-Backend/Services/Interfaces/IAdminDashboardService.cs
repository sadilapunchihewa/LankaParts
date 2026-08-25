using LankaParts_Backend.DTOs.Admin;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();
        Task<List<AdminUserDto>> GetUsersAsync(string? role, bool? isActive, string? search);
        Task<AdminUserDto> SetUserActiveAsync(int adminUserId, int userId, bool isActive);
        Task<List<LowStockPartDto>> GetLowStockAsync(int threshold);
    }
}
