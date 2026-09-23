using BookStoreApi.Models;

namespace BookStoreApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Book> book { get; set; } = new List<Book>();
    }
}