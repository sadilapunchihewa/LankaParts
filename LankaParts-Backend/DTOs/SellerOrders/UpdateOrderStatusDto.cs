using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.SellerOrders
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
