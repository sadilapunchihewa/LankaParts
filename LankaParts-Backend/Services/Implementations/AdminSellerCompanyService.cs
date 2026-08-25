using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Admin;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class AdminSellerCompanyService : IAdminSellerCompanyService
    {
        private readonly ApplicationDbContext _context;

        public AdminSellerCompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminSellerCompanyDto>> GetAllAsync(string? status)
        {
            var query = _context.SellerCompanies
                .AsNoTracking()
                .Include(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                var normalizedStatus = NormalizeStatus(status);
                query = query.Where(c => c.Status == normalizedStatus);
            }

            return await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => Map(c))
                .ToListAsync();
        }

        public async Task<AdminSellerCompanyDto?> GetByIdAsync(int companyId)
        {
            var company = await _context.SellerCompanies
                .AsNoTracking()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == companyId);
            return company is null ? null : Map(company);
        }

        public Task<AdminSellerCompanyDto> ApproveAsync(
            int companyId, int adminUserId, string? note) =>
            ReviewAsync(companyId, adminUserId, CompanyStatuses.Approved, note);

        public Task<AdminSellerCompanyDto> RejectAsync(
            int companyId, int adminUserId, string? note) =>
            ReviewAsync(companyId, adminUserId, CompanyStatuses.Rejected, note);

        private async Task<AdminSellerCompanyDto> ReviewAsync(
            int companyId, int adminUserId, string status, string? note)
        {
            var adminExists = await _context.Users
                .AnyAsync(u => u.Id == adminUserId && u.Role == UserRoles.Admin && u.IsActive);
            if (!adminExists)
                throw new UnauthorizedAccessException("Active admin account not found.");

            var company = await _context.SellerCompanies
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == companyId)
                ?? throw new KeyNotFoundException("Seller company application not found.");

            if (company.Status != CompanyStatuses.Pending)
                throw new InvalidOperationException("Only pending applications can be reviewed.");

            company.Status = status;
            company.ReviewedByUserId = adminUserId;
            company.ReviewedAt = DateTime.UtcNow;
            company.ReviewNote = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            await _context.SaveChangesAsync();
            return Map(company);
        }

        private static string NormalizeStatus(string status)
        {
            if (status.Equals(CompanyStatuses.Pending, StringComparison.OrdinalIgnoreCase))
                return CompanyStatuses.Pending;
            if (status.Equals(CompanyStatuses.Approved, StringComparison.OrdinalIgnoreCase))
                return CompanyStatuses.Approved;
            if (status.Equals(CompanyStatuses.Rejected, StringComparison.OrdinalIgnoreCase))
                return CompanyStatuses.Rejected;

            throw new ArgumentException("Status must be Pending, Approved, or Rejected.");
        }

        private static AdminSellerCompanyDto Map(SellerCompany company) => new()
        {
            Id = company.Id,
            SellerUserId = company.UserId,
            SellerName = $"{company.User.FirstName} {company.User.LastName}".Trim(),
            SellerEmail = company.User.Email,
            CompanyName = company.CompanyName,
            BusinessRegistrationNumber = company.BusinessRegistrationNumber,
            Address = company.Address,
            City = company.City,
            PhoneNumber = company.PhoneNumber,
            Status = company.Status,
            CreatedAt = company.CreatedAt,
            ReviewedByUserId = company.ReviewedByUserId,
            ReviewedAt = company.ReviewedAt,
            ReviewNote = company.ReviewNote
        };
    }
}
