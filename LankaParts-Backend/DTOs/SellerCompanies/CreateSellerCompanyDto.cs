using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.DTOs.SellerCompanies
{
    public class CreateSellerCompanyDto
    {
        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string BusinessRegistrationNumber { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
