using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Roles
{
    public class GetRoleDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
