using LibraryManagementSystem.Console.Models;

namespace LibraryManagementSystem.Console.Repositories;

/// <summary>
/// Определяет CRUD операции для пользователей.
/// </summary>
public interface IUserRepository
{
    int Create(User user);

    IEnumerable<User> GetAll();

    User? GetById(int id);

    bool Update(User user);

    bool Delete(int id);
}