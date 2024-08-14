namespace BookApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; } = new HashSet<Book>() { };

    }
}
