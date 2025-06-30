using Cibrary_Backend.Contexts;
using Cibrary_Backend.Errors;
using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;

public class CirculationRepository
{
    private readonly CirculationDBContext _context;

    public CirculationRepository(CirculationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Circulation>?> GetBooks()
    {
        var booksCirculation = await _context.Circulation.Where(p => p.BookStatus == BookStatus.pending).ToListAsync();
        return booksCirculation;
    }

    //     public async Task<Circulation> CheckInBook(int barcodeId)
    //     {

    //     }
}
