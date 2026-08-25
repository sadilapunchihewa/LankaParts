using System.ComponentModel.DataAnnotations;
using LankaParts_Backend.Helpers;

namespace LankaParts_Backend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SparePartId { get; set; }
        public int SellerCompanyId { get; set; }

        [Required, MaxLength(150)]
        public string PartName { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string PartNumber { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }

        [Required, MaxLength(20)]
        public string FulfillmentStatus { get; set; } = OrderStatuses.Pending;

        public DateTime? ProcessingAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Order Order { get; set; } = null!;
        public SparePart SparePart { get; set; } = null!;
        public SellerCompany SellerCompany { get; set; } = null!;
    }
}
