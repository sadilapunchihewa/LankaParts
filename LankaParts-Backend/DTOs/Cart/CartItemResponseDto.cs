namespace LankaParts_Backend.DTOs.Cart
{
    public class CartItemResponseDto
    {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int AvailableStock { get; set; }
        public decimal LineTotal { get; set; }
    }
}
