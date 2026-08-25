using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.Admin
{
    public class ReviewSellerCompanyDto
    {
        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
