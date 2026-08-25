using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.SellerCompanies;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class SellerCompanyService : ISellerCompanyService
    {
        private readonly ApplicationDbContext _context;

        public SellerCompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SellerCompanyResponseDto> CreateAsync(int userId, CreateSellerCompanyDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null || user.Role != UserRoles.Seller)
                throw new InvalidOperationException("Only sellers can create a company profile.");

            if (await _context.SellerCompanies.AnyAsync(c => c.UserId == userId))
                throw new InvalidOperationException("You already have a company profile.");

            var registrationNumber = dto.BusinessRegistrationNumber.Trim();
            if (await _context.SellerCompanies.AnyAsync(
                    c => c.BusinessRegistrationNumber == registrationNumber))
                throw new InvalidOperationException("This business registration number is already in use.");

            var company = new SellerCompany
            {
                UserId = userId,
                CompanyName = dto.CompanyName.Trim(),
                BusinessRegistrationNumber = registrationNumber,
                Address = dto.Address.Trim(),
                City = dto.City.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                Status = CompanyStatuses.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.SellerCompanies.Add(company);
            await _context.SaveChangesAsync();
            return Map(company);
        }

        public async Task<SellerCompanyResponseDto?> GetMineAsync(int userId)
        {
            var company = await _context.SellerCompanies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId);
            return company is null ? null : Map(company);
        }

        private static SellerCompanyResponseDto Map(SellerCompany company) => new()
        {
            Id = company.Id,
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
