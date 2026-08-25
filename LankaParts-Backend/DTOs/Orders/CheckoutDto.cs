using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.Orders
{
    public class CheckoutDto
    {
        [Required, MaxLength(250)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ShippingCity { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string ContactPhone { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = "CashOnDelivery";
    }
}
