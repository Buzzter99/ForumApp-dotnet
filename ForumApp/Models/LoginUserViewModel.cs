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
        [Required]
        [Compare(nameof(Password),ErrorMessage ="Passwords do not match!")]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}
