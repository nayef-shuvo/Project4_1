using System.ComponentModel.DataAnnotations;

namespace Project4_1.Models
{
    public class AuthModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
