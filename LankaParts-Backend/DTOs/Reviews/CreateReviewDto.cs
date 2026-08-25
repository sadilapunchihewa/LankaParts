using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.Reviews
{
    public class CreateReviewDto
    {
        [Range(1, int.MaxValue)]
        public int OrderItemId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}
