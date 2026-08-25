namespace LankaParts_Backend.DTOs.SellerOrders
{
    public class SellerOrderItemDto
    {
        public int OrderItemId { get; set; }
        public int SparePartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public string FulfillmentStatus { get; set; } = string.Empty;
    }
}
