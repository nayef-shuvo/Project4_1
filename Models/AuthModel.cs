using System.ComponentModel.DataAnnotations;

namespace Project4_1.Models
{
    public class AuthModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;
    }
}
