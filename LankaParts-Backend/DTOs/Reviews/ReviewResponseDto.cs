namespace LankaParts_Backend.DTOs.Reviews
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool VerifiedPurchase { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
