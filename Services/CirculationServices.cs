using Cibrary_Backend.Models;
using Cibrary_Backend.Repository;

namespace Cibrary_Backend.Services;

public class CirculationServices
{
    private readonly CirculationRepository _repository;
    private readonly BookCopyRepository _bookCopyRepo;

    public CirculationServices(CirculationRepository repository, BookCopyRepository bookCopyRepo)
    {
        _repository = repository;
        _bookCopyRepo = bookCopyRepo;
    }

    public async Task<List<Circulation>?> GetPendingBooks()
    {
        var pendingBooks = await _repository.GetBooks();

        return pendingBooks;
    }

    public async Task<Circulation> CheckoutBook(int bookId, string userId)
    {
        Circulation res = await _repository.CheckoutABookAsync(bookId, userId);

        return res;

    }

    public async Task<Circulation?> CompleteCheckout(int id)
    {
        var res = await _repository.CompleteCheckout(id);
        return res; 
}
    public async Task<BookCopy?> GetBookCopyByIdAsync(int id)
    {
        var res = await _bookCopyRepo.GetABookCopyById(id);
        return res;
    }
}