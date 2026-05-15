using Dapper;
using LibraryManagementSystem.Console.Database;

namespace LibraryManagementSystem.Tests;

/// <summary>
/// Вспомогательные методы для тестов.
/// </summary>
public static class TestHelper
{
    /// <summary>
    /// Очищает тестовые данные.
    /// </summary>
    public static void CleanupUsers()
    {
        DatabaseManager databaseManager = new();

        using var connection =
            databaseManager.CreateConnection();

        connection.Execute(
            """
            DELETE FROM books;
            DELETE FROM users;
            """
        );
    }
}