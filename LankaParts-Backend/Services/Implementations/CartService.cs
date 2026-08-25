using LankaParts_Backend.Data;
using LankaParts_Backend.DTOs.Cart;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Models;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LankaParts_Backend.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context) => _context = context;

        public async Task<CartResponseDto> GetAsync(int customerUserId)
        {
            var items = await CartQuery(customerUserId).OrderBy(i => i.CreatedAt).ToListAsync();
            return MapCart(items);
        }

        public async Task<CartResponseDto> AddAsync(int customerUserId, AddCartItemDto dto)
        {
            await EnsureCustomer(customerUserId);
            var part = await _context.SpareParts
                .Include(p => p.SellerCompany)
                .FirstOrDefaultAsync(p => p.Id == dto.SparePartId)
                ?? throw new KeyNotFoundException("Spare part not found.");

            ValidateAvailablePart(part);

            var item = await _context.CartItems.FirstOrDefaultAsync(i =>
                i.CustomerUserId == customerUserId && i.SparePartId == dto.SparePartId);
            var newQuantity = (item?.Quantity ?? 0) + dto.Quantity;
            ValidateQuantity(newQuantity, part.StockQuantity);

            if (item is null)
            {
                _context.CartItems.Add(new CartItem
                {
                    CustomerUserId = customerUserId,
                    SparePartId = dto.SparePartId,
                    Quantity = dto.Quantity,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                item.Quantity = newQuantity;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return await GetAsync(customerUserId);
        }

        public async Task<CartResponseDto> UpdateAsync(
            int customerUserId, int cartItemId, int quantity)
        {
            var item = await _context.CartItems
                .Include(i => i.SparePart).ThenInclude(p => p.SellerCompany)
                .FirstOrDefaultAsync(i => i.Id == cartItemId && i.CustomerUserId == customerUserId)
                ?? throw new KeyNotFoundException("Cart item not found.");

            ValidateAvailablePart(item.SparePart);
            ValidateQuantity(quantity, item.SparePart.StockQuantity);
            item.Quantity = quantity;
            item.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetAsync(customerUserId);
        }

        public async Task RemoveAsync(int customerUserId, int cartItemId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(i =>
                i.Id == cartItemId && i.CustomerUserId == customerUserId)
                ?? throw new KeyNotFoundException("Cart item not found.");
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task ClearAsync(int customerUserId)
        {
            var items = await _context.CartItems
                .Where(i => i.CustomerUserId == customerUserId).ToListAsync();
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        private IQueryable<CartItem> CartQuery(int customerUserId) =>
            _context.CartItems.AsNoTracking()
                .Include(i => i.SparePart).ThenInclude(p => p.SellerCompany)
                .Where(i => i.CustomerUserId == customerUserId);

        private async Task EnsureCustomer(int customerUserId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == customerUserId &&
                    u.Role == UserRoles.Customer && u.IsActive))
                throw new UnauthorizedAccessException("Active customer account not found.");
        }

        private static void ValidateAvailablePart(SparePart part)
        {
            if (!part.IsActive || part.SellerCompany.Status != CompanyStatuses.Approved)
                throw new InvalidOperationException("This spare part is not currently available.");
            if (part.StockQuantity < 1)
                throw new InvalidOperationException("This spare part is out of stock.");
        }

        private static void ValidateQuantity(int quantity, int stock)
        {
            if (quantity < 1) throw new ArgumentException("Quantity must be at least 1.");
            if (quantity > 100) throw new ArgumentException("A cart item cannot exceed 100 units.");
            if (quantity > stock) throw new InvalidOperationException($"Only {stock} unit(s) are available.");
        }

        private static CartResponseDto MapCart(List<CartItem> items)
        {
            var response = new CartResponseDto
            {
                Items = items.Select(i => new CartItemResponseDto
                {
                    Id = i.Id,
                    SparePartId = i.SparePartId,
                    PartName = i.SparePart.Name,
                    PartNumber = i.SparePart.PartNumber,
                    CompanyName = i.SparePart.SellerCompany.CompanyName,
                    ImageUrl = i.SparePart.ImageUrl,
                    UnitPrice = i.SparePart.Price,
                    Quantity = i.Quantity,
                    AvailableStock = i.SparePart.StockQuantity,
                    LineTotal = i.SparePart.Price * i.Quantity
                }).ToList()
            };
            response.TotalItems = response.Items.Sum(i => i.Quantity);
            response.Subtotal = response.Items.Sum(i => i.LineTotal);
            return response;
        }
    }
}
