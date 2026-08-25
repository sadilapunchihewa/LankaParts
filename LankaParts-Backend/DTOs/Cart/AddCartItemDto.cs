using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.Cart
{
    public class AddCartItemDto
    {
        [Range(1, int.MaxValue)]
        public int SparePartId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
