using System.ComponentModel.DataAnnotations;
using LankaParts_Backend.Helpers;

namespace LankaParts_Backend.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Required, MaxLength(30)]
        public string PaymentNumber { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Method { get; set; } = PaymentMethods.CashOnDelivery;

        [Required, MaxLength(20)]
        public string Status { get; set; } = PaymentStatuses.Pending;

        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Order Order { get; set; } = null!;
    }
}
