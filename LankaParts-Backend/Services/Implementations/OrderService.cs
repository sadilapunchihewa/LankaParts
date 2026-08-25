using System.Data;
using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Orders;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context) => _context = context;

        public async Task<OrderResponseDto> CheckoutAsync(int customerUserId, CheckoutDto dto)
        {
            if (!dto.PaymentMethod.Equals(PaymentMethods.CashOnDelivery,
                    StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Only CashOnDelivery is currently supported.");

            await using var transaction = await _context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            var cartItems = await _context.CartItems
                .Include(i => i.SparePart).ThenInclude(p => p.SellerCompany)
                .Where(i => i.CustomerUserId == customerUserId)
                .ToListAsync();

            if (cartItems.Count == 0)
                throw new InvalidOperationException("Your cart is empty.");

            foreach (var item in cartItems)
            {
                var part = item.SparePart;
                if (!part.IsActive || part.SellerCompany.Status != CompanyStatuses.Approved)
                    throw new InvalidOperationException($"{part.Name} is no longer available.");
                if (item.Quantity > part.StockQuantity)
                    throw new InvalidOperationException(
                        $"Only {part.StockQuantity} unit(s) of {part.Name} are available.");
            }

            var order = new Order
            {
                OrderNumber = CreateOrderNumber(),
                CustomerUserId = customerUserId,
                ShippingAddress = dto.ShippingAddress.Trim(),
                ShippingCity = dto.ShippingCity.Trim(),
                ContactPhone = dto.ContactPhone.Trim(),
                Status = OrderStatuses.Pending,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var cartItem in cartItems)
            {
                var part = cartItem.SparePart;
                var lineTotal = part.Price * cartItem.Quantity;
                order.Items.Add(new OrderItem
                {
                    SparePartId = part.Id,
                    SellerCompanyId = part.SellerCompanyId,
                    PartName = part.Name,
                    PartNumber = part.PartNumber,
                    UnitPrice = part.Price,
                    Quantity = cartItem.Quantity,
                    LineTotal = lineTotal,
                    FulfillmentStatus = OrderStatuses.Pending
                });
                order.TotalAmount += lineTotal;
                part.StockQuantity -= cartItem.Quantity;
                part.UpdatedAt = DateTime.UtcNow;
            }

            order.Payment = new Payment
            {
                PaymentNumber = CreatePaymentNumber(),
                Method = PaymentMethods.CashOnDelivery,
                Status = PaymentStatuses.Pending,
                Amount = order.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetMineByIdAsync(customerUserId, order.Id)
                ?? throw new InvalidOperationException("Could not load the created order.");
        }

        public async Task<List<OrderResponseDto>> GetMineAsync(int customerUserId)
        {
            var orders = await OrderQuery().Where(o => o.CustomerUserId == customerUserId)
                .OrderByDescending(o => o.CreatedAt).ToListAsync();
            return orders.Select(Map).ToList();
        }

        public async Task<OrderResponseDto?> GetMineByIdAsync(int customerUserId, int orderId)
        {
            var order = await OrderQuery().FirstOrDefaultAsync(o =>
                o.Id == orderId && o.CustomerUserId == customerUserId);
            return order is null ? null : Map(order);
        }

        public async Task<OrderResponseDto> CancelAsync(int customerUserId, int orderId)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            var order = await _context.Orders.Include(o => o.Items)
                .ThenInclude(i => i.SparePart)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerUserId == customerUserId)
                ?? throw new KeyNotFoundException("Order not found.");

            if (order.Status != OrderStatuses.Pending)
                throw new InvalidOperationException("Only pending orders can be cancelled.");

            foreach (var item in order.Items)
            {
                item.SparePart.StockQuantity += item.Quantity;
                item.SparePart.UpdatedAt = DateTime.UtcNow;
            }

            order.Status = OrderStatuses.Cancelled;
            order.CancelledAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            if (order.Payment is not null && order.Payment.Status == PaymentStatuses.Pending)
            {
                order.Payment.Status = PaymentStatuses.Cancelled;
                order.Payment.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetMineByIdAsync(customerUserId, orderId)
                ?? throw new InvalidOperationException("Could not load the cancelled order.");
        }

        private IQueryable<Order> OrderQuery() => _context.Orders.AsNoTracking()
            .Include(o => o.Items).ThenInclude(i => i.SellerCompany)
            .Include(o => o.Payment);

        private static string CreateOrderNumber() =>
            $"LP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        private static string CreatePaymentNumber() =>
            $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        private static OrderResponseDto Map(Order order) => new()
        {
            Id = order.Id, OrderNumber = order.OrderNumber,
            ShippingAddress = order.ShippingAddress, ShippingCity = order.ShippingCity,
            ContactPhone = order.ContactPhone, Status = order.Status,
            TotalAmount = order.TotalAmount, CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt, CancelledAt = order.CancelledAt,
            PaymentMethod = order.Payment?.Method ?? string.Empty,
            PaymentStatus = order.Payment?.Status ?? string.Empty,
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                Id = i.Id, SparePartId = i.SparePartId,
                SellerCompanyId = i.SellerCompanyId,
                CompanyName = i.SellerCompany.CompanyName,
                PartName = i.PartName, PartNumber = i.PartNumber,
                UnitPrice = i.UnitPrice, Quantity = i.Quantity, LineTotal = i.LineTotal,
                FulfillmentStatus = i.FulfillmentStatus,
                ProcessingAt = i.ProcessingAt, ShippedAt = i.ShippedAt,
                DeliveredAt = i.DeliveredAt
            }).ToList()
        };
    }
}
