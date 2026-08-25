using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int CustomerUserId { get; set; }
        public int SparePartId { get; set; }
        public int OrderItemId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User CustomerUser { get; set; } = null!;
        public SparePart SparePart { get; set; } = null!;
        public OrderItem OrderItem { get; set; } = null!;
    }
}
