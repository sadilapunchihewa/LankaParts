using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.SellerOrders;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class SellerOrderService : ISellerOrderService
    {
        private readonly ApplicationDbContext _context;
        public SellerOrderService(ApplicationDbContext context) => _context = context;

        public async Task<List<SellerOrderResponseDto>> GetAllAsync(
            int sellerUserId, string? status)
        {
            var companyId = await GetCompanyId(sellerUserId);
            var normalizedStatus = string.IsNullOrWhiteSpace(status) ? null : NormalizeStatus(status);

            var orders = await BaseQuery(companyId)
                .Where(o => normalizedStatus == null || o.Items.Any(i =>
                    i.SellerCompanyId == companyId && i.FulfillmentStatus == normalizedStatus))
                .OrderByDescending(o => o.CreatedAt).ToListAsync();

            return orders.Select(o => Map(o, companyId)).ToList();
        }

        public async Task<SellerOrderResponseDto?> GetByIdAsync(int sellerUserId, int orderId)
        {
            var companyId = await GetCompanyId(sellerUserId);
            var order = await BaseQuery(companyId).FirstOrDefaultAsync(o => o.Id == orderId);
            return order is null ? null : Map(order, companyId);
        }

        public async Task<SellerOrderResponseDto> UpdateStatusAsync(
            int sellerUserId, int orderId, string newStatus)
        {
            var companyId = await GetCompanyId(sellerUserId);
            var normalizedStatus = NormalizeStatus(newStatus);
            var order = await _context.Orders
                .Include(o => o.CustomerUser)
                .Include(o => o.Items)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderId &&
                    o.Items.Any(i => i.SellerCompanyId == companyId))
                ?? throw new KeyNotFoundException("Seller order not found.");

            if (order.Status == OrderStatuses.Cancelled)
                throw new InvalidOperationException("A cancelled order cannot be updated.");

            var sellerItems = order.Items.Where(i => i.SellerCompanyId == companyId).ToList();
            foreach (var item in sellerItems)
            {
                EnsureValidTransition(item.FulfillmentStatus, normalizedStatus);
                item.FulfillmentStatus = normalizedStatus;
                item.UpdatedAt = DateTime.UtcNow;
                if (normalizedStatus == OrderStatuses.Processing) item.ProcessingAt ??= DateTime.UtcNow;
                if (normalizedStatus == OrderStatuses.Shipped) item.ShippedAt ??= DateTime.UtcNow;
                if (normalizedStatus == OrderStatuses.Delivered) item.DeliveredAt ??= DateTime.UtcNow;
            }

            order.Status = CalculateOverallStatus(order.Items);
            order.UpdatedAt = DateTime.UtcNow;

            if (order.Status == OrderStatuses.Delivered &&
                order.Payment?.Method == PaymentMethods.CashOnDelivery &&
                order.Payment.Status == PaymentStatuses.Pending)
            {
                order.Payment.Status = PaymentStatuses.Paid;
                order.Payment.PaidAt = DateTime.UtcNow;
                order.Payment.UpdatedAt = DateTime.UtcNow;
                order.Payment.TransactionReference = $"COD-{order.OrderNumber}";
            }
            await _context.SaveChangesAsync();

            return await GetByIdAsync(sellerUserId, orderId)
                ?? throw new InvalidOperationException("Could not load the updated order.");
        }

        private IQueryable<Order> BaseQuery(int companyId) => _context.Orders.AsNoTracking()
            .Include(o => o.CustomerUser)
            .Include(o => o.Items)
            .Where(o => o.Items.Any(i => i.SellerCompanyId == companyId));

        private async Task<int> GetCompanyId(int sellerUserId)
        {
            var company = await _context.SellerCompanies.AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == sellerUserId &&
                    c.Status == CompanyStatuses.Approved);
            return company?.Id
                ?? throw new InvalidOperationException("An approved company profile is required.");
        }

        private static string NormalizeStatus(string status)
        {
            if (status.Equals(OrderStatuses.Pending, StringComparison.OrdinalIgnoreCase))
                return OrderStatuses.Pending;
            if (status.Equals(OrderStatuses.Processing, StringComparison.OrdinalIgnoreCase))
                return OrderStatuses.Processing;
            if (status.Equals(OrderStatuses.Shipped, StringComparison.OrdinalIgnoreCase))
                return OrderStatuses.Shipped;
            if (status.Equals(OrderStatuses.Delivered, StringComparison.OrdinalIgnoreCase))
                return OrderStatuses.Delivered;
            throw new ArgumentException("Status must be Pending, Processing, Shipped, or Delivered.");
        }

        private static void EnsureValidTransition(string current, string next)
        {
            if (current == next) return;
            var valid = current switch
            {
                OrderStatuses.Pending => next == OrderStatuses.Processing,
                OrderStatuses.Processing => next == OrderStatuses.Shipped,
                OrderStatuses.Shipped => next == OrderStatuses.Delivered,
                _ => false
            };
            if (!valid)
                throw new InvalidOperationException($"Cannot change order status from {current} to {next}.");
        }

        private static string CalculateOverallStatus(IEnumerable<OrderItem> items)
        {
            var statuses = items.Select(i => i.FulfillmentStatus).ToList();
            if (statuses.All(s => s == OrderStatuses.Delivered)) return OrderStatuses.Delivered;
            if (statuses.All(s => s is OrderStatuses.Shipped or OrderStatuses.Delivered))
                return OrderStatuses.Shipped;
            if (statuses.Any(s => s != OrderStatuses.Pending)) return OrderStatuses.Processing;
            return OrderStatuses.Pending;
        }

        private static SellerOrderResponseDto Map(Order order, int companyId)
        {
            var items = order.Items.Where(i => i.SellerCompanyId == companyId)
                .Select(i => new SellerOrderItemDto
                {
                    OrderItemId = i.Id, SparePartId = i.SparePartId,
                    PartName = i.PartName, PartNumber = i.PartNumber,
                    UnitPrice = i.UnitPrice, Quantity = i.Quantity,
                    LineTotal = i.LineTotal, FulfillmentStatus = i.FulfillmentStatus
                }).ToList();
            return new SellerOrderResponseDto
            {
                OrderId = order.Id, OrderNumber = order.OrderNumber,
                CustomerName = $"{order.CustomerUser.FirstName} {order.CustomerUser.LastName}".Trim(),
                CustomerEmail = order.CustomerUser.Email, ContactPhone = order.ContactPhone,
                ShippingAddress = order.ShippingAddress, ShippingCity = order.ShippingCity,
                Status = items.Select(i => i.FulfillmentStatus).Distinct().Count() == 1
                    ? items[0].FulfillmentStatus : "Mixed",
                SellerSubtotal = items.Sum(i => i.LineTotal), CreatedAt = order.CreatedAt,
                Items = items
            };
        }
    }
}
