using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Roles
{
    public class DeleteRoleDto
    {
        [Required]
        public int Id { get; set; }
    }
}
