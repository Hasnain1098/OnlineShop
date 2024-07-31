using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DataModels
{
    public class User : IdentityUser
    {
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
