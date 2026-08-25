using LankaParts_Backend.DTOs.Parts;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface ISparePartService
    {
        Task<List<PartCategoryDto>> GetCategoriesAsync();
        Task<List<SparePartResponseDto>> BrowseAsync(string? search, int? categoryId);
        Task<SparePartResponseDto?> GetPublicByIdAsync(int partId);
        Task<List<SparePartResponseDto>> GetMineAsync(int sellerUserId);
        Task<SparePartResponseDto> CreateAsync(int sellerUserId, UpsertSparePartDto dto);
        Task<SparePartResponseDto> UpdateAsync(int sellerUserId, int partId, UpsertSparePartDto dto);
        Task DeactivateAsync(int sellerUserId, int partId);
    }
}
