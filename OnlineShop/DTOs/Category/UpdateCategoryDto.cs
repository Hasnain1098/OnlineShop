using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public int? CategoryID { get; set; }

        [Required]
        public string? NewName { get; set; }

        [Required]
        public string? NewDescription { get; set; }
    }
}
