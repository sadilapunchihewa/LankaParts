namespace LankaParts_Backend.DTOs.Parts
{
    public class SparePartResponseDto
    {
        public int Id { get; set; }
        public int SellerCompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
