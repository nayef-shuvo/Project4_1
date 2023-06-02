using System.ComponentModel.DataAnnotations;

namespace Project4_1.Models.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public Rank Rank { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

    }
}
