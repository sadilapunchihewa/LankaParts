namespace LankaParts_Backend.DTOs.Orders
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public int SellerCompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public string FulfillmentStatus { get; set; } = string.Empty;
        public DateTime? ProcessingAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
