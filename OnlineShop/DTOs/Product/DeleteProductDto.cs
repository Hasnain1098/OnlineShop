using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Product
{
    public class DeleteProductDto
    {
        [Required]
        public int Id { get; set; }
    }
}
