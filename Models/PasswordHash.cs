using System.ComponentModel.DataAnnotations;

namespace Project4_1.Models
{
    public class PasswordHash
    {
        [Key]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Hash { get; set; } = string.Empty;
    }
}