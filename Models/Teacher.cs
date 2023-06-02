using System.ComponentModel.DataAnnotations;

namespace Project4_1.Models
{
    public class Teacher
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Key]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public Rank Rank { get; set; }

        public string Department { get; set; } = "CSE";

    }
}
