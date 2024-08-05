using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
