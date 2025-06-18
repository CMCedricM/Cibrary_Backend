using Cibrary_Backend.Contexts;
using Cibrary_Backend.Errors;
using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    public async Task<BookProfile?> GetBookById(int id)
    {
        var aBook = await _context.Books.FirstOrDefaultAsync(b => id == b.ID);

        return aBook;
    }
    public async Task<BookProfile?> GetBookByISBN(string isbn)
    {
        var getBook = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);

        return getBook;

    }

    public async Task<BookProfile> CreateBook(BookProfile book)
    {
        var aBook = await GetBookByISBN(book.Isbn);
        if (aBook != null) throw new ConflictFound($"{(book.Title != string.Empty ? book.Title : "book")} with {book.Isbn} exists ",
         book.Isbn, book.Title);

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<BookProfile?> UpdateBook(int id, BookProfile book)
    {
        var aBook = await GetBookById(id);
        if (aBook == null) return null;

        var properties = typeof(BookProfile).GetProperties();

        foreach (var prop in properties)
        {
            if (prop.Name.Equals("id", StringComparison.OrdinalIgnoreCase) ||
            prop.Name.Equals("isbn", StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenFieldException($"Atempted to update illegal fields ${prop.Name}", [prop.Name]);
            }

            var newValue = prop.GetValue(book);
            if (newValue != null) prop.SetValue(aBook, newValue);
        }

        await _context.SaveChangesAsync();

        return aBook;

    }


    public async Task<List<BookProfile>> FindBook(string item)
    {
        var books = await _context.Books.Where(p => EF.Functions.ILike(p.Title, $"%{item}%")).ToListAsync();

        return books;
    }
}