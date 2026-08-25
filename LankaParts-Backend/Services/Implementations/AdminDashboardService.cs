using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Admin;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext _context;
        public AdminDashboardService(ApplicationDbContext context) => _context = context;

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var ratings = await _context.Reviews.AsNoTracking()
                .Select(r => (double?)r.Rating).ToListAsync();

            return new AdminDashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                ActiveCustomers = await _context.Users.CountAsync(u =>
                    u.Role == UserRoles.Customer && u.IsActive),
                ActiveSellers = await _context.Users.CountAsync(u =>
                    u.Role == UserRoles.Seller && u.IsActive),
                PendingSellerCompanies = await _context.SellerCompanies.CountAsync(c =>
                    c.Status == CompanyStatuses.Pending),
                ApprovedSellerCompanies = await _context.SellerCompanies.CountAsync(c =>
                    c.Status == CompanyStatuses.Approved),
                ActivePartListings = await _context.SpareParts.CountAsync(p => p.IsActive),
                LowStockListings = await _context.SpareParts.CountAsync(p =>
                    p.IsActive && p.StockQuantity <= 5),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatuses.Pending),
                DeliveredOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatuses.Delivered),
                CancelledOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatuses.Cancelled),
                PaidRevenue = await _context.Payments
                    .Where(p => p.Status == PaymentStatuses.Paid)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0,
                PendingPaymentAmount = await _context.Payments
                    .Where(p => p.Status == PaymentStatuses.Pending)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0,
                TotalReviews = ratings.Count,
                AverageMarketplaceRating = ratings.Count == 0
                    ? 0 : Math.Round(ratings.Average() ?? 0, 1)
            };
        }

        public async Task<List<AdminUserDto>> GetUsersAsync(
            string? role, bool? isActive, string? search)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(role))
            {
                var normalizedRole = NormalizeRole(role);
                query = query.Where(u => u.Role == normalizedRole);
            }
            if (isActive.HasValue) query = query.Where(u => u.IsActive == isActive.Value);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u => u.Email.ToLower().Contains(term) ||
                    u.FirstName.ToLower().Contains(term) || u.LastName.ToLower().Contains(term));
            }

            var users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
            return users.Select(MapUser).ToList();
        }

        public async Task<AdminUserDto> SetUserActiveAsync(
            int adminUserId, int userId, bool isActive)
        {
            if (adminUserId == userId && !isActive)
                throw new InvalidOperationException("You cannot deactivate your own admin account.");

            var user = await _context.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");
            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return MapUser(user);
        }

        public Task<List<LowStockPartDto>> GetLowStockAsync(int threshold)
        {
            if (threshold < 0 || threshold > 1000)
                throw new ArgumentException("Threshold must be between 0 and 1000.");

            return _context.SpareParts.AsNoTracking()
                .Where(p => p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .Select(p => new LowStockPartDto
                {
                    SparePartId = p.Id, PartName = p.Name,
                    PartNumber = p.PartNumber,
                    CompanyName = p.SellerCompany.CompanyName,
                    StockQuantity = p.StockQuantity, IsActive = p.IsActive
                }).ToListAsync();
        }

        private static string NormalizeRole(string role)
        {
            if (role.Equals(UserRoles.Customer, StringComparison.OrdinalIgnoreCase)) return UserRoles.Customer;
            if (role.Equals(UserRoles.Seller, StringComparison.OrdinalIgnoreCase)) return UserRoles.Seller;
            if (role.Equals(UserRoles.Admin, StringComparison.OrdinalIgnoreCase)) return UserRoles.Admin;
            throw new ArgumentException("Role must be Customer, Seller, or Admin.");
        }

        private static AdminUserDto MapUser(User u) => new()
        {
            Id = u.Id, FirstName = u.FirstName, LastName = u.LastName,
            Email = u.Email, PhoneNumber = u.PhoneNumber, Role = u.Role,
            IsActive = u.IsActive, CreatedAt = u.CreatedAt
        };
    }
}
