namespace LankaParts_Backend.DTOs.Cart
{
    public class CartResponseDto
    {
        public List<CartItemResponseDto> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public decimal Subtotal { get; set; }
    }
}
