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

    public async Task<BookProfile?> GetBookById(int id)
    {
        var book = await _repository.GetBookById(id);

        return book;
    }

    public async Task<BookProfile?> GetBookByISBN(string isbn)
    {
        var book = await _repository.GetBookByISBN(isbn);

        return book;
    }

    public async Task<BookProfile?> GetBookAsnyc(string isbn)
    {
        BookProfile? aBook = await _repository.GetBookByISBN(isbn);

        return aBook;

    }

    public async Task<BookProfile> CreateBookAsync(BookProfile aBook)
    {

        var createdBook = await _repository.CreateBook(aBook);

        return createdBook;

    }

    public async Task<BookProfile?> UpdateBook(int id, BookProfile req)
    {
        var updatedBook = await _repository.UpdateBook(id, req);

        return updatedBook;

    }


}