using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CustomerUserId { get; set; }
        public int SparePartId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User CustomerUser { get; set; } = null!;
        public SparePart SparePart { get; set; } = null!;
    }
}
