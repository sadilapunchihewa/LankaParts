namespace LankaParts_Backend.DTOs.Admin
{
    public class LowStockPartDto
    {
        public int SparePartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
