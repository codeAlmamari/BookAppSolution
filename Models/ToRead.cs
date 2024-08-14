using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class ToRead
    {
        [Key]
        public int Id { get; set; }
        public string State { get; set; }

        public int BookId { get; set; }
        public Book Books { get; set; }

    }
}
