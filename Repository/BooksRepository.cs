using Cibrary_Backend.Contexts;
using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;

public class BooksRepository
{
    private readonly BooksDBContext _context;


    public BooksRepository(BooksDBContext context)
    {
        _context = context;
    }

    public async Task<int> GetBookCount()
    {
        int count = await _context.Books.CountAsync();

        return count;
    }
    public async Task<BookProfile?> GetBook(string isbn)
    {
        var getBook = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);

        return getBook;

    }

    public async Task<BookProfile> CreateBook(BookProfile book)
    {
        var aBook = await GetBook(book.ISBN);
        if (aBook != null) return aBook;

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book;
    }
}