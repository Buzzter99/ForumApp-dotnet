using ForumApp.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models
{
    public class LoginUserViewModel : IUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
