﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DTOs.Roles
{
    public class UpdateRoleDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}