using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Auther {  get; set; }
        public DateOnly Date { get; set; }
        [Required]
        [Range(1,5)]
        public int Rate { get; set; }
        [Required]
        public string ImageURL { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
