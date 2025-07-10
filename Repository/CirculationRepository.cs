using Cibrary_Backend.Contexts;
using Cibrary_Backend.dtos;
using Cibrary_Backend.Errors;
using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;

public class CirculationRepository
{
    private readonly CirculationDBContext _context;
    private readonly BookCopyDBContext _booksCopyContext;

    private readonly UserDBContext _userDbContext;

    public CirculationRepository(CirculationDBContext context, BookCopyDBContext bookCopyContext, UserDBContext userDbContext)
    {
        _context = context;
        _booksCopyContext = bookCopyContext;
        _userDbContext = userDbContext;
    }

    public async Task<List<Circulation>?> GetBooks()
    {
        var booksCirculation = await _context.Circulation.Where(p => p.Status == BookStatus.pending).ToListAsync();
        return booksCirculation;
    }

    // For Checking in a book, we need to return the information for the ui to display
    public async Task<Circulation?> RetrieveInfo(int bookId)
    {
        var bookCirculation = await _context.Circulation.Include(b => b.User).Include(b => b.BookCopy).FirstOrDefaultAsync(p => p.Id == bookId);
        if (bookCirculation == null) { throw new DataNotFound("Could not locate records for the requested book id.", "", ""); }
        return bookCirculation;
    }

    // To check out a book
    public async Task<CheckoutResponse> CheckoutABookAsync(int bookId, string user_id)
    {
        // Verify the book exists in our database
        var book = await _booksCopyContext.BookCopy.Include(b => b.Book).FirstOrDefaultAsync(p => p.ID == bookId);
        if (book == null) throw new DataNotFound("Could not locate records for the requested book id.", "", "");

        // Verify the user we are checking it out to exists
        var user = await _userDbContext.Users.FirstOrDefaultAsync(p => p.auth0id == user_id);
        if (user == null) throw new NotAllowed($"Could not locate user with provided id {user_id}", user_id);

        // Verify that there is no pending or checked out status of the book
        var findCirculation = await _context.Circulation.FirstOrDefaultAsync(p => p.BookCopyId == bookId && (p.BookCopy.Status != BookStatus.returned || p.Status == BookStatus.pending || p.Status == BookStatus.checked_out));
        if (findCirculation != null) throw new ConflictFound("This book already has a checkout request", book.Book.Isbn, book.Book.Title);
        // Run the checkout flow
        // 1. Create a record in the Circulation 
        Circulation newCirculation = new Circulation
        {
            UserId = user.id,
            BookCopyId = book.ID,


            CheckoutDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.Date.AddDays(14),
            ReturnDate = DateTime.UtcNow,
            Status = BookStatus.pending
        };

        await _context.Circulation.AddAsync(newCirculation);
        // 2. Save the changes
        await _context.SaveChangesAsync();

        CheckoutResponse res = new()
        {
            Id = newCirculation.Id,
            UserAuth0Id = user.auth0id,
            UserEmail = user.email,
            BookCopy = book
        };

        return res;
    }

    public async Task<Circulation?> CompleteCheckout(int id)
    {
        var checkOutData = await _context.Circulation.FirstOrDefaultAsync(p => p.Id == id);
        if (checkOutData == null) return null;

        checkOutData.Status = BookStatus.checked_out;

        await _context.SaveChangesAsync();

        return checkOutData;
    }

    public async Task<Boolean> CancelCheckout(int id)
    {
        await _context.Circulation.Where(p => p.Id == id).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();

        return true;
    }

}
