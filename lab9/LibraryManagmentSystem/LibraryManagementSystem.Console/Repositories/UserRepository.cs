using Dapper;
using LibraryManagementSystem.Console.Database;
using LibraryManagementSystem.Console.Models;
using Serilog;

namespace LibraryManagementSystem.Console.Repositories;

/// <summary>
/// Выполняет CRUD операции для таблицы users.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DatabaseManager _databaseManager;

    /// <summary>
    /// Создает экземпляр репозитория пользователей.
    /// </summary>
    /// <param name="databaseManager">
    /// Менеджер подключения к БД.
    /// </param>
    public UserRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Добавляет пользователя в БД.
    /// </summary>
    /// <param name="user">Объект пользователя.</param>
    /// <returns>ID новой записи.</returns>
    public int Create(User user)
    {
        try
        {
            using var connection = _databaseManager.CreateConnection();

            string sql =
                """
                INSERT INTO users
                (
                    username,
                    password_hash,
                    email
                )
                VALUES
                (
                    @Username,
                    @PasswordHash,
                    @Email
                )
                RETURNING id;
                """;

            int id = connection.ExecuteScalar<int>(sql, user);

            Log.Information(
                "Создан пользователь {Username}",
                user.Username
            );

            return id;
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Ошибка при создании пользователя"
            );

            throw;
        }
    }

    /// <summary>
    /// Возвращает всех пользователей.
    /// </summary>
    /// <returns>Коллекция пользователей.</returns>
    public IEnumerable<User> GetAll()
    {
        try
        {
            using var connection = _databaseManager.CreateConnection();

            string sql = "SELECT * FROM users";

            return connection.Query<User>(sql);
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Ошибка получения пользователей"
            );

            throw;
        }
    }

    /// <summary>
    /// Возвращает пользователя по ID.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <returns>Пользователь или null.</returns>
    public User? GetById(int id)
    {
        try
        {
            using var connection = _databaseManager.CreateConnection();

            string sql =
                """
                SELECT *
                FROM users
                WHERE id = @Id
                """;

            return connection.QueryFirstOrDefault<User>(
                sql,
                new { Id = id }
            );
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Ошибка получения пользователя"
            );

            throw;
        }
    }

    /// <summary>
    /// Обновляет пользователя.
    /// </summary>
    /// <param name="user">Обновленный пользователь.</param>
    /// <returns>
    /// true - успешно.
    /// false - запись не найдена.
    /// </returns>
    public bool Update(User user)
    {
        try
        {
            using var connection = _databaseManager.CreateConnection();

            string sql =
                """
                UPDATE users
                SET
                    username = @Username,
                    email = @Email
                WHERE id = @Id
                """;

            int affectedRows = connection.Execute(sql, user);

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Ошибка обновления пользователя"
            );

            throw;
        }
    }

    /// <summary>
    /// Удаляет пользователя.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <returns>
    /// true - успешно.
    /// false - запись не найдена.
    /// </returns>
    public bool Delete(int id)
    {
        try
        {
            using var connection = _databaseManager.CreateConnection();

            string sql =
                """
                DELETE FROM users
                WHERE id = @Id
                """;

            int affectedRows = connection.Execute(
                sql,
                new { Id = id }
            );

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Ошибка удаления пользователя"
            );

            throw;
        }
    }
}