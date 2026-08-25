using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.Models
{
    public class SparePart
    {
        public int Id { get; set; }
        public int SellerCompanyId { get; set; }
        public int CategoryId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Brand { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(80)]
        public string? VehicleMake { get; set; }

        [MaxLength(80)]
        public string? VehicleModel { get; set; }

        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public SellerCompany SellerCompany { get; set; } = null!;
        public PartCategory Category { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
