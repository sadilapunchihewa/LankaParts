using System.ComponentModel.DataAnnotations;

namespace LankaParts_Backend.Models
{
    public class PartCategory
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<SparePart> SpareParts { get; set; } = new List<SparePart>();
    }
}
