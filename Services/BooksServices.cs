using Cibrary_Backend.Repository;
using Cibrary_Backend.Models;

namespace Cibrary_Backend.Services;

public class BooksServices
{
    private readonly BooksRepository _repository;

    public BooksServices(BooksRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> GetBookCountAsync()
    {
        int count = await _repository.GetBookCount();

        return count;
    }

    public async Task<BookProfile?> GetBookAsnyc(string isbn)
    {
        BookProfile? aBook = await _repository.GetBook(isbn);

        return aBook;

    }

    public async Task<BookProfile> CreateBookAsync(BookProfile aBook)
    {
        var book = await _repository.GetBook(aBook.ISBN);
        if (book != null) { return book; }

        var createdBook = await _repository.CreateBook(aBook);

        return createdBook;

    }


}