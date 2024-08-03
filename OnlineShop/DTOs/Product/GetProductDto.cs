using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Product
{
    public class GetProductDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
