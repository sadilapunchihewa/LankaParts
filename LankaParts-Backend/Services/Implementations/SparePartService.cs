using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Parts;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class SparePartService : ISparePartService
    {
        private readonly ApplicationDbContext _context;

        public SparePartService(ApplicationDbContext context) => _context = context;

        public Task<List<PartCategoryDto>> GetCategoriesAsync() =>
            _context.PartCategories.AsNoTracking().OrderBy(c => c.Name)
                .Select(c => new PartCategoryDto(c.Id, c.Name)).ToListAsync();

        public async Task<List<SparePartResponseDto>> BrowseAsync(string? search, int? categoryId)
        {
            var query = BaseQuery().Where(p => p.IsActive &&
                p.SellerCompany.Status == CompanyStatuses.Approved);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) ||
                    p.PartNumber.ToLower().Contains(term) ||
                    (p.Brand != null && p.Brand.ToLower().Contains(term)) ||
                    (p.VehicleMake != null && p.VehicleMake.ToLower().Contains(term)) ||
                    (p.VehicleModel != null && p.VehicleModel.ToLower().Contains(term)));
            }

            var parts = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return parts.Select(Map).ToList();
        }

        public async Task<SparePartResponseDto?> GetPublicByIdAsync(int partId)
        {
            var part = await BaseQuery().FirstOrDefaultAsync(p => p.Id == partId && p.IsActive &&
                p.SellerCompany.Status == CompanyStatuses.Approved);
            return part is null ? null : Map(part);
        }

        public async Task<List<SparePartResponseDto>> GetMineAsync(int sellerUserId)
        {
            var parts = await BaseQuery().Where(p => p.SellerCompany.UserId == sellerUserId)
                .OrderByDescending(p => p.CreatedAt).ToListAsync();
            return parts.Select(Map).ToList();
        }

        public async Task<SparePartResponseDto> CreateAsync(int sellerUserId, UpsertSparePartDto dto)
        {
            var company = await GetApprovedCompany(sellerUserId);
            await ValidateCategory(dto.CategoryId);
            await ValidatePartNumber(company.Id, dto.PartNumber, null);

            var part = new SparePart { SellerCompanyId = company.Id, CreatedAt = DateTime.UtcNow };
            Apply(part, dto);
            _context.SpareParts.Add(part);
            await _context.SaveChangesAsync();
            part.SellerCompany = company;
            part.Category = await _context.PartCategories.FindAsync(part.CategoryId) ?? null!;
            return Map(part);
        }

        public async Task<SparePartResponseDto> UpdateAsync(
            int sellerUserId, int partId, UpsertSparePartDto dto)
        {
            var part = await _context.SpareParts
                .Include(p => p.SellerCompany)
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == partId && p.SellerCompany.UserId == sellerUserId)
                ?? throw new KeyNotFoundException("Spare part listing not found.");

            if (part.SellerCompany.Status != CompanyStatuses.Approved)
                throw new InvalidOperationException("Your company must be approved to manage listings.");

            await ValidateCategory(dto.CategoryId);
            await ValidatePartNumber(part.SellerCompanyId, dto.PartNumber, part.Id);
            Apply(part, dto);
            part.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            part.Category = await _context.PartCategories.FindAsync(part.CategoryId) ?? null!;
            return Map(part);
        }

        public async Task DeactivateAsync(int sellerUserId, int partId)
        {
            var part = await _context.SpareParts.Include(p => p.SellerCompany)
                .FirstOrDefaultAsync(p => p.Id == partId && p.SellerCompany.UserId == sellerUserId)
                ?? throw new KeyNotFoundException("Spare part listing not found.");
            part.IsActive = false;
            part.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private IQueryable<SparePart> BaseQuery() => _context.SpareParts.AsNoTracking()
            .Include(p => p.SellerCompany).Include(p => p.Category).Include(p => p.Reviews);

        private async Task<SellerCompany> GetApprovedCompany(int sellerUserId) =>
            await _context.SellerCompanies.FirstOrDefaultAsync(c => c.UserId == sellerUserId &&
                c.Status == CompanyStatuses.Approved)
            ?? throw new InvalidOperationException("An approved company profile is required.");

        private async Task ValidateCategory(int categoryId)
        {
            if (!await _context.PartCategories.AnyAsync(c => c.Id == categoryId))
                throw new ArgumentException("Invalid part category.");
        }

        private async Task ValidatePartNumber(int companyId, string partNumber, int? excludedId)
        {
            var normalized = partNumber.Trim().ToLower();
            if (await _context.SpareParts.AnyAsync(p => p.SellerCompanyId == companyId &&
                p.PartNumber.ToLower() == normalized && p.Id != excludedId))
                throw new InvalidOperationException("This part number already exists in your company.");
        }

        private static void Apply(SparePart part, UpsertSparePartDto dto)
        {
            part.Name = dto.Name.Trim();
            part.PartNumber = dto.PartNumber.Trim();
            part.CategoryId = dto.CategoryId;
            part.Brand = Clean(dto.Brand);
            part.Description = Clean(dto.Description);
            part.VehicleMake = Clean(dto.VehicleMake);
            part.VehicleModel = Clean(dto.VehicleModel);
            part.YearFrom = dto.YearFrom;
            part.YearTo = dto.YearTo;
            part.Price = dto.Price;
            part.StockQuantity = dto.StockQuantity;
            part.ImageUrl = Clean(dto.ImageUrl);
        }

        private static string? Clean(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static SparePartResponseDto Map(SparePart p) => new()
        {
            Id = p.Id, SellerCompanyId = p.SellerCompanyId,
            CompanyName = p.SellerCompany.CompanyName, CategoryId = p.CategoryId,
            CategoryName = p.Category.Name, Name = p.Name, PartNumber = p.PartNumber,
            Brand = p.Brand, Description = p.Description, VehicleMake = p.VehicleMake,
            VehicleModel = p.VehicleModel, YearFrom = p.YearFrom, YearTo = p.YearTo,
            Price = p.Price, StockQuantity = p.StockQuantity, ImageUrl = p.ImageUrl,
            IsActive = p.IsActive, CreatedAt = p.CreatedAt, UpdatedAt = p.UpdatedAt,
            AverageRating = p.Reviews.Count == 0 ? 0 : Math.Round(p.Reviews.Average(r => r.Rating), 1),
            ReviewCount = p.Reviews.Count
        };
    }
}
