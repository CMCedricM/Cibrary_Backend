using Cibrary_Backend.Models;
using Cibrary_Backend.Repository;

namespace Cibrary_Backend.Services;

public class CirculationServices
{
    private readonly CirculationRepository _repository;

    public CirculationServices(CirculationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Circulation>?> GetPendingBooks()
    {
        var pendingBooks = await _repository.GetBooks();

        return pendingBooks;
    }
}