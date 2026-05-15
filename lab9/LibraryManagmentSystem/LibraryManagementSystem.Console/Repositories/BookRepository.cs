using Dapper;
using LibraryManagementSystem.Console.Database;
using LibraryManagementSystem.Console.Models;
using Serilog;

namespace LibraryManagementSystem.Console.Repositories;

/// <summary>
/// Создает экземпляр репозитория книг.
/// </summary>
/// <param name="databaseManager">
/// Менеджер подключения к базе данных.
/// </param>
public class BookRepository : IBookRepository
{
    private readonly DatabaseManager _databaseManager;

    public BookRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Добавляет книгу в базу данных.
    /// </summary>
    /// <param name="book">
    /// Объект книги.
    /// </param>
    /// <returns>
    /// ID созданной книги.
    /// </returns>
    /// <exception cref="Exception">
    /// Возникает при ошибке добавления книги.
    /// </exception>
    public int Create(Book book)
    {
        try
        {
            using var connection = _databaseManager.CreateConnection();

            string sql = """
                INSERT INTO books
                (
                    title,
                    author,
                    publication_year,
                    owner_id
                )
                VALUES
                (
                    @Title,
                    @Author,
                    @PublicationYear,
                    @OwnerId
                )
                RETURNING id;
                """;

            int id = connection.ExecuteScalar<int>(sql, book);

            Log.Information("Создана книга {Title}", book.Title);

            return id;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка создания книги");

            throw;
        }
    }

    /// <summary>
    /// Возвращает список всех книг.
    /// </summary>
    /// <returns>
    /// Коллекция книг.
    /// </returns>
    public IEnumerable<Book> GetAll()
    {
        using var connection = _databaseManager.CreateConnection();

        return connection.Query<Book>("SELECT * FROM books");
    }

    /// <summary>
    /// Возвращает книгу по ID.
    /// </summary>
    /// <param name="id">
    /// ID книги.
    /// </param>
    /// <returns>
    /// Найденная книга или null.
    /// </returns>
    public Book? GetById(int id)
    {
        using var connection = _databaseManager.CreateConnection();

        return connection.QueryFirstOrDefault<Book>(
            """
            SELECT *
            FROM books
            WHERE id = @Id
            """,
            new { Id = id }
        );
    }

    /// <summary>
    /// Обновляет данные книги.
    /// </summary>
    /// <param name="book">
    /// Обновленный объект книги.
    /// </param>
    /// <returns>
    /// true - обновление успешно.
    /// false - книга не найдена.
    /// </returns>
    public bool Update(Book book)
    {
        using var connection = _databaseManager.CreateConnection();

        int affectedRows = connection.Execute(
            """
            UPDATE books
            SET
                title = @Title,
                author = @Author,
                publication_year = @PublicationYear
            WHERE id = @Id
            """,
            book
        );

        return affectedRows > 0;
    }

    /// <summary>
    /// Удаляет книгу по ID.
    /// </summary>
    /// <param name="id">
    /// ID книги.
    /// </param>
    /// <returns>
    /// true - удаление успешно.
    /// false - книга не найдена.
    /// </returns>
    public bool Delete(int id)
    {
        using var connection = _databaseManager.CreateConnection();

        int affectedRows = connection.Execute(
            """
            DELETE FROM books
            WHERE id = @Id
            """,
            new { Id = id }
        );

        return affectedRows > 0;
    }
}
