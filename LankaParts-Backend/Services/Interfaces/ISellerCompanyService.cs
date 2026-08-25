using LankaParts_Backend.DTOs.SellerCompanies;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface ISellerCompanyService
    {
        Task<SellerCompanyResponseDto> CreateAsync(int userId, CreateSellerCompanyDto dto);
        Task<SellerCompanyResponseDto?> GetMineAsync(int userId);
    }
}
