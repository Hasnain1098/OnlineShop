using OnlineShop.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Category
{
    public class GetCategoryDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public ICollection<GetProductDto> Products { get; set; } = new List<GetProductDto>();
    }
}
