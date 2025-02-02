using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumApp.Data.Models
{
    public class User
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
