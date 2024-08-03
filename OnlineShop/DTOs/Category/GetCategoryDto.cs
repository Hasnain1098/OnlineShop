using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Category
{
    public class GetCategoryDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
