using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Product
{
    public class UpdateProductDto
    {
        [Required]
        public int? ProductID { get; set; }

        [Required]
        public string? NewName { get; set; }

        [Required]
        public string? NewDescription { get; set; }
    }
}
