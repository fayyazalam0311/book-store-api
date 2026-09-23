using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Data;
using BookStoreApi.Models;
using BookStoreApi.DTOs;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public BooksController(BookStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookQueryParameters parameters)
        {
            var query = _context.Books.AsQueryable();

            if(!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(b =>
                    b.Title.ToLower().Contains(parameters.Search.ToLower()) ||
                    (b.Author != null && b.Author.Name.ToLower().Contains(parameters.Search.ToLower())));
            }

            var totalCount = await query.CountAsync();

            var books = await query
                .OrderBy(b => b.Id)
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    AuthorName = b.Author != null ? b.Author.Name : string.Empty,
                    CategoryName = b.Category != null ? b.Category.Name : string.Empty
                })
                .ToListAsync();

            Response.Headers.Append("X-Total-Count", totalCount.ToString());

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            var book = await _context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    AuthorName = b.Author != null ? b.Author.Name : string.Empty,
                    CategoryName = b.Category != null ? b.Category.Name : string.Empty
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto dto)
        {
            var authorExists = await _context.Authors.FindAsync(dto.AuthorId);
            if (authorExists == null)
            {
                return BadRequest($"Invalid AuthorId: {dto.AuthorId}");
            }

            var categoryExists = await _context.Categories.FindAsync(dto.CategoryId);
            if (categoryExists == null)
            {
                return BadRequest($"Invalid CategoryId: {dto.CategoryId}");
            }

            var newBook = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            var responseBook = new BookDto
                {
                    Id = newBook.Id,
                    Title = newBook.Title,
                    ISBN = newBook.ISBN,
                    PublishedYear = newBook.PublishedYear,
                    AuthorName = authorExists.Name,
                    CategoryName = categoryExists.Name
                };

            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, responseBook);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook (int id, CreateBookDto dto)
        {
            var bookToUpdate = await _context.Books.FindAsync(id);
            if (bookToUpdate == null)
            {
                return NotFound();
            }

            var authorExists = await _context.Authors.AnyAsync(a => a.Id == dto.AuthorId);
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);

            if (!authorExists || !categoryExists)
            {
                return BadRequest("Invalid AuthorId or CategoryId.");
            }

            bookToUpdate.Title = dto.Title;
            bookToUpdate.ISBN = dto.ISBN;
            bookToUpdate.PublishedYear = dto.PublishedYear;
            bookToUpdate.AuthorId = dto.AuthorId;
            bookToUpdate.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook (int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
