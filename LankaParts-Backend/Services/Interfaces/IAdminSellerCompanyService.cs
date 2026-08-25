using LankaParts_Backend.DTOs.Admin;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IAdminSellerCompanyService
    {
        Task<List<AdminSellerCompanyDto>> GetAllAsync(string? status);
        Task<AdminSellerCompanyDto?> GetByIdAsync(int companyId);
        Task<AdminSellerCompanyDto> ApproveAsync(int companyId, int adminUserId, string? note);
        Task<AdminSellerCompanyDto> RejectAsync(int companyId, int adminUserId, string? note);
    }
}
