using LankaParts_Backend.DTOs.Cart;

namespace LankaParts_Backend.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> GetAsync(int customerUserId);
        Task<CartResponseDto> AddAsync(int customerUserId, AddCartItemDto dto);
        Task<CartResponseDto> UpdateAsync(int customerUserId, int cartItemId, int quantity);
        Task RemoveAsync(int customerUserId, int cartItemId);
        Task ClearAsync(int customerUserId);
    }
}
