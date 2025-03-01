﻿using ForumApp.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models
{
    public class RegisterUserViewModel : IUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
