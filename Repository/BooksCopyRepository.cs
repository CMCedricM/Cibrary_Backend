using System.Security.Principal;
using Cibrary_Backend.Contexts;
using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Cibrary_Backend.Repository;

public class BookCopyRepository
{
    private readonly BookCopyDBContext _context;
    public BookCopyRepository(BookCopyDBContext context)
    {
        _context = context;
    }

    public async Task<BookCopy?> CreateABookCopy(Book book)
    {
        List<BookCopy> booksToCreate = [];
        for (int i = 0; i < book.TotalCnt; i++)
        {
            BookCopy newBookItem = new BookCopy
            {
                BookId = book.ID,
                Status = BookStatus.returned
            };
            booksToCreate.Add(newBookItem);
        }

        await _context.BookCopy.AddRangeAsync(booksToCreate);
        await _context.SaveChangesAsync();

        return booksToCreate.First();

    }

    public async Task<BookCopy?> GetABookCopyById(int id)
    {
        var aCopy = await _context.BookCopy.Include(p => p.Book).FirstOrDefaultAsync(p => p.ID == id);
        return aCopy;
    }

}