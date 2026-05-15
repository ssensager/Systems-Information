using Dapper;
using LibraryManagementSystem.Console.Database;
using LibraryManagementSystem.Console.Models;
using Serilog;

namespace LibraryManagementSystem.Console.Repositories;

/// <summary>
/// Выполняет CRUD операции для books.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly DatabaseManager _databaseManager;

    public BookRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public int Create(Book book)
    {
        try
        {
            using var connection =
                _databaseManager.CreateConnection();

            string sql =
                """
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

            int id =
                connection.ExecuteScalar<int>(sql, book);

            Log.Information(
                "Создана книга {Title}",
                book.Title
            );

            return id;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка создания книги");

            throw;
        }
    }

    public IEnumerable<Book> GetAll()
    {
        using var connection =
            _databaseManager.CreateConnection();

        return connection.Query<Book>(
            "SELECT * FROM books"
        );
    }

    public Book? GetById(int id)
    {
        using var connection =
            _databaseManager.CreateConnection();

        return connection.QueryFirstOrDefault<Book>(
            """
            SELECT *
            FROM books
            WHERE id = @Id
            """,
            new { Id = id }
        );
    }

    public bool Update(Book book)
    {
        using var connection =
            _databaseManager.CreateConnection();

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

    public bool Delete(int id)
    {
        using var connection =
            _databaseManager.CreateConnection();

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