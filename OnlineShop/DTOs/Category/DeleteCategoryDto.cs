using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Category
{
    public class DeleteCategoryDto
    {

        [Required]
        public int Id { get; set; }

    }
}
