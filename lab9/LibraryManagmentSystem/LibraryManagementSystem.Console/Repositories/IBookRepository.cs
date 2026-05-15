using LibraryManagementSystem.Console.Models;

namespace LibraryManagementSystem.Console.Repositories;

/// <summary>
/// CRUD операции для книг.
/// </summary>
public interface IBookRepository
{
    int Create(Book book);

    IEnumerable<Book> GetAll();

    Book? GetById(int id);

    bool Update(Book book);

    bool Delete(int id);
}