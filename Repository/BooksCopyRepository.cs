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
                Book_ID = book.ID
            };
            booksToCreate.Add(newBookItem);
        }

        await _context.BookCopy.AddRangeAsync(booksToCreate);
        await _context.SaveChangesAsync();

        return booksToCreate.First();

    }

}