using Cibrary_Backend.dtos;
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

    public async Task<CheckoutResponse> CheckoutBook(int bookId, string userId)
    {
        CheckoutResponse res = await _repository.CheckoutABookAsync(bookId, userId);

        return res;

    }

    public async Task<Circulation?> CompleteCheckout(int id)
    {
        var res = await _repository.CompleteCheckout(id);
        return res;
    }

    public async Task<Boolean> CancelCheckout(int id)
    {
        var data = await _repository.CancelCheckout(id);

        return data;
    }
    public async Task<BookCopy?> GetBookCopyByIdAsync(int id)
    {
        var res = await _bookCopyRepo.GetABookCopyById(id);
        return res;
    }

    public async Task<CheckInResponse> CheckinBook(int bookId, string patronAuth0Id)
    {
        var res = await _repository.CheckinBook(bookId, patronAuth0Id);
        return res;
    }

}