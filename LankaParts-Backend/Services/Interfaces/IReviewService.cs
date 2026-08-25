using LankaParts_Backend.DTOs.Reviews;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewResponseDto>> GetForPartAsync(int sparePartId);
        Task<List<ReviewResponseDto>> GetMineAsync(int customerUserId);
        Task<ReviewResponseDto> CreateAsync(int customerUserId, CreateReviewDto dto);
        Task<ReviewResponseDto> UpdateAsync(int customerUserId, int reviewId, UpdateReviewDto dto);
        Task DeleteAsync(int customerUserId, int reviewId);
    }
}
