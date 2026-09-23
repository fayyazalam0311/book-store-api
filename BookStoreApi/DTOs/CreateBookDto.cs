namespace BookStoreApi.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }


        public int AuthorId { get; set; }
        public int CategoryId { get; set; }


        public AuthorDto? Author { get; set; }
        public CategoryDto? Category { get; set; }

    }
}
