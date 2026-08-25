using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Reviews;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        public ReviewService(ApplicationDbContext context) => _context = context;

        public async Task<List<ReviewResponseDto>> GetForPartAsync(int sparePartId)
        {
            var reviews = await ReviewQuery().Where(r => r.SparePartId == sparePartId)
                .OrderByDescending(r => r.CreatedAt).ToListAsync();
            return reviews.Select(Map).ToList();
        }

        public async Task<List<ReviewResponseDto>> GetMineAsync(int customerUserId)
        {
            var reviews = await ReviewQuery().Where(r => r.CustomerUserId == customerUserId)
                .OrderByDescending(r => r.CreatedAt).ToListAsync();
            return reviews.Select(Map).ToList();
        }

        public async Task<ReviewResponseDto> CreateAsync(int customerUserId, CreateReviewDto dto)
        {
            var orderItem = await _context.OrderItems
                .Include(i => i.Order)
                .FirstOrDefaultAsync(i => i.Id == dto.OrderItemId &&
                    i.Order.CustomerUserId == customerUserId)
                ?? throw new KeyNotFoundException("Purchased order item not found.");

            if (orderItem.FulfillmentStatus != OrderStatuses.Delivered)
                throw new InvalidOperationException("You can review a part only after it is delivered.");
            if (await _context.Reviews.AnyAsync(r => r.OrderItemId == dto.OrderItemId))
                throw new InvalidOperationException("This purchased item has already been reviewed.");

            var review = new Review
            {
                CustomerUserId = customerUserId,
                SparePartId = orderItem.SparePartId,
                OrderItemId = orderItem.Id,
                Rating = dto.Rating,
                Comment = Clean(dto.Comment),
                CreatedAt = DateTime.UtcNow
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return await GetById(review.Id) ?? throw new InvalidOperationException("Could not load review.");
        }

        public async Task<ReviewResponseDto> UpdateAsync(
            int customerUserId, int reviewId, UpdateReviewDto dto)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r =>
                r.Id == reviewId && r.CustomerUserId == customerUserId)
                ?? throw new KeyNotFoundException("Review not found.");
            review.Rating = dto.Rating;
            review.Comment = Clean(dto.Comment);
            review.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetById(review.Id) ?? throw new InvalidOperationException("Could not load review.");
        }

        public async Task DeleteAsync(int customerUserId, int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r =>
                r.Id == reviewId && r.CustomerUserId == customerUserId)
                ?? throw new KeyNotFoundException("Review not found.");
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        private async Task<ReviewResponseDto?> GetById(int reviewId)
        {
            var review = await ReviewQuery().FirstOrDefaultAsync(r => r.Id == reviewId);
            return review is null ? null : Map(review);
        }

        private IQueryable<Review> ReviewQuery() => _context.Reviews.AsNoTracking()
            .Include(r => r.CustomerUser);

        private static string? Clean(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static ReviewResponseDto Map(Review r) => new()
        {
            Id = r.Id, SparePartId = r.SparePartId,
            CustomerName = $"{r.CustomerUser.FirstName} " +
                (string.IsNullOrEmpty(r.CustomerUser.LastName) ? string.Empty : $"{r.CustomerUser.LastName[0]}."),
            Rating = r.Rating, Comment = r.Comment, VerifiedPurchase = true,
            CreatedAt = r.CreatedAt, UpdatedAt = r.UpdatedAt
        };
    }
}
