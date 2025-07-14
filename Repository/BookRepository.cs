using Cibrary_Backend.Contexts;
using Cibrary_Backend.Errors;
using Cibrary_Backend.Models;
using Cibrary_Backend.Repository;
using Microsoft.EntityFrameworkCore;

public class BooksRepository
{
    private readonly BookDBContext _context;
    private readonly BookCopyRepository _bookCopyRepository;

    public BooksRepository(BookDBContext context, BookCopyRepository bookCopyRepository)
    {
        _context = context;
        _bookCopyRepository = bookCopyRepository;
    }

    public async Task<int> GetBookCount()
    {
        int count = await _context.Books.CountAsync();

        return count;
    }

    public async Task<Book?> GetBookByInformationById(int id)
    {
        var aBook = await _context.Books.FirstOrDefaultAsync(b => id == b.ID);

        return aBook;
    }
    public async Task<Book?> GetBookInformationByISBN(string isbn)
    {
        var getBook = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);

        return getBook;

    }

    public async Task<Book> CreateBook(Book book)
    {
        var aBook = await GetBookInformationByISBN(book.Isbn);
        if (aBook != null) throw new ConflictFound($"{(book.Title != string.Empty ? book.Title : "book")} with {book.Isbn} exists ",
         book.Isbn, book.Title);

        // Since we are creating the book here we can jsut assume the available count
        // is the total count
        book.AvailabilityCnt = book.TotalCnt;

        // Create the book first in the Book table
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        await _bookCopyRepository.CreateABookCopy(book);

        return book;
    }

    public async Task<Book?> UpdateBook(int id, Book book)
    {
        var aBook = await GetBookByInformationById(id);
        if (aBook == null) return null;

        var properties = typeof(Book).GetProperties();

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


    public async Task<List<Book>?> FindBook(BookSearch item)
    {
        if (!string.IsNullOrWhiteSpace(item.Isbn))
        {

            var query = await _context.Books.Where(p => EF.Functions.ILike(p.Isbn, $"%{item.Isbn}%")).ToListAsync();
            return query;
        }
        if (!string.IsNullOrWhiteSpace(item.Title))
        {
            var query = await _context.Books.Where(p => EF.Functions.ILike(p.Title, $"%{item.Title}%")).ToListAsync();
            return query;
        }

        return null;

    }
}