using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Product
{
    public class UpdateProductDto
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
