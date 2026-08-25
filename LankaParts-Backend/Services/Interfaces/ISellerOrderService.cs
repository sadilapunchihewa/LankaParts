using LankaParts_Backend.DTOs.SellerOrders;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface ISellerOrderService
    {
        Task<List<SellerOrderResponseDto>> GetAllAsync(int sellerUserId, string? status);
        Task<SellerOrderResponseDto?> GetByIdAsync(int sellerUserId, int orderId);
        Task<SellerOrderResponseDto> UpdateStatusAsync(
            int sellerUserId, int orderId, string newStatus);
    }
}
