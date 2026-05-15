using LibraryManagementSystem.Console.Database;

namespace LibraryManagementSystem.Tests;

/// <summary>
/// Проверяет успешное подключение к PostgreSQL.
/// </summary>
public class DatabaseTests
{
    [Fact]
    public void Database_Should_Connect_Successfully()
    {
        // Arrange
        DatabaseManager databaseManager = new();

        // Act
        using var connection = databaseManager.CreateConnection();

        // Assert
        Assert.NotNull(connection);

        Assert.Equal(System.Data.ConnectionState.Open, connection.State);
    }
}
