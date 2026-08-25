using System.ComponentModel.DataAnnotations;
using LankaParts_Backend.Helpers;

namespace LankaParts_Backend.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerUserId { get; set; }

        [Required, MaxLength(250)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ShippingCity { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string ContactPhone { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = OrderStatuses.Pending;

        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public User CustomerUser { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
    }
}
