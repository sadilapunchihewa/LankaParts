using LankaParts_Backend.DTOs.Payments;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentResponseDto>> GetMineAsync(int customerUserId);
        Task<PaymentResponseDto?> GetByOrderAsync(int customerUserId, int orderId);
    }
}
