using System.ComponentModel.DataAnnotations;

namespace Project4_1.Models.Items
{
    public class Item
    {
        [Required]
        [MaxLength(100)]
        [Key]
        public string Title { get; set; } = string.Empty;
        [Required]
        public Dictionary<string, int> SubItem { get; set; } = new Dictionary<string, int>();
    }
}
