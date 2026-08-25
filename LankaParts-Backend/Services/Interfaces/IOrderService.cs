using LankaParts_Backend.DTOs.Orders;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CheckoutAsync(int customerUserId, CheckoutDto dto);
        Task<List<OrderResponseDto>> GetMineAsync(int customerUserId);
        Task<OrderResponseDto?> GetMineByIdAsync(int customerUserId, int orderId);
        Task<OrderResponseDto> CancelAsync(int customerUserId, int orderId);
    }
}
