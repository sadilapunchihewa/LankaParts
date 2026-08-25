using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Payments;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        public PaymentService(ApplicationDbContext context) => _context = context;

        public async Task<List<PaymentResponseDto>> GetMineAsync(int customerUserId)
        {
            var payments = await _context.Payments.AsNoTracking()
                .Include(p => p.Order)
                .Where(p => p.Order.CustomerUserId == customerUserId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return payments.Select(Map).ToList();
        }

        public async Task<PaymentResponseDto?> GetByOrderAsync(int customerUserId, int orderId)
        {
            var payment = await _context.Payments.AsNoTracking()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId &&
                    p.Order.CustomerUserId == customerUserId);
            return payment is null ? null : Map(payment);
        }

        private static PaymentResponseDto Map(Payment payment) => new()
        {
            Id = payment.Id, OrderId = payment.OrderId,
            OrderNumber = payment.Order.OrderNumber,
            PaymentNumber = payment.PaymentNumber, Method = payment.Method,
            Status = payment.Status, Amount = payment.Amount,
            TransactionReference = payment.TransactionReference,
            CreatedAt = payment.CreatedAt, PaidAt = payment.PaidAt
        };
    }
}
