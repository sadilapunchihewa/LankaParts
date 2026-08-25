namespace LankaParts_Backend.DTOs.SellerOrders
{
    public class SellerOrderResponseDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal SellerSubtotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SellerOrderItemDto> Items { get; set; } = new();
    }
}
