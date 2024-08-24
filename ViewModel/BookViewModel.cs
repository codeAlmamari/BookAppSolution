using System.ComponentModel.DataAnnotations;

namespace BookApp.ViewModel
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Auther { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }

        [Required]
        public IFormFile Image { get; set; }  // This will be used to upload the image

        public int CategoryId { get; set; }
    }
}
