using System.ComponentModel.DataAnnotations;
using LankaParts_Backend.Helpers;

namespace LankaParts_Backend.Models
{
    public class SellerCompany
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string BusinessRegistrationNumber { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = CompanyStatuses.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ReviewedByUserId { get; set; }

        public DateTime? ReviewedAt { get; set; }

        [MaxLength(500)]
        public string? ReviewNote { get; set; }

        public User User { get; set; } = null!;

        public User? ReviewedByUser { get; set; }

        public ICollection<SparePart> SpareParts { get; set; } = new List<SparePart>();
    }
}
