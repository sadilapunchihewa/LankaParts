using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.Parts
{
    public class UpsertSparePartDto : IValidatableObject
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string PartNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(80)]
        public string? VehicleMake { get; set; }

        [MaxLength(80)]
        public string? VehicleModel { get; set; }

        [Range(1900, 2200)]
        public int? YearFrom { get; set; }

        [Range(1900, 2200)]
        public int? YearTo { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Url, MaxLength(500)]
        public string? ImageUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (YearFrom.HasValue && YearTo.HasValue && YearFrom > YearTo)
                yield return new ValidationResult(
                    "YearFrom cannot be later than YearTo.",
                    new[] { nameof(YearFrom), nameof(YearTo) });
        }
    }
}
