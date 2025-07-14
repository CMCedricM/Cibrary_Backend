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

    public async Task<Book?> GetBookInformationByIdAsync(int id)
    {
        var book = await _repository.GetBookByInformationById(id);

        return book;
    }

    public async Task<Book?> GetBookInformationByISBNAsync(string isbn)
    {
        var book = await _repository.GetBookInformationByISBN(isbn);

        return book;
    }

    public async Task<Book?> GetBookAsnyc(string isbn)
    {
        Book? aBook = await _repository.GetBookInformationByISBN(isbn);

        return aBook;

    }

    public async Task<Book> CreateBookAsync(Book aBook)
    {

        var createdBook = await _repository.CreateBook(aBook);

        return createdBook;

    }

    public async Task<Book?> UpdateBook(int id, Book req)
    {
        var updatedBook = await _repository.UpdateBook(id, req);

        return updatedBook;

    }

    public async Task<List<Book>?> FindABook(BookSearch item)
    {
        var results = await _repository.FindBook(item);

        return results;
    }


}