using Npgsql;

namespace LibraryManagementSystem.Console.Database;

/// <summary>
/// Управляет подключением к PostgreSQL.
/// </summary>
public class DatabaseManager
{
    /// <summary>
    /// Создает подключение к БД.
    /// </summary>
    /// <returns>Открытое подключение PostgreSQL.</returns>
    /// <exception cref="Exception">
    /// Возникает при ошибке подключения.
    /// </exception>
    public NpgsqlConnection CreateConnection()
    {
        try
        {
            var connection =
                new NpgsqlConnection(
                    DbConfig.CONNECTION_STRING
                );

            connection.Open();

            return connection;
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Ошибка подключения к PostgreSQL: {ex.Message}"
            );
        }
    }
}