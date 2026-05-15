using LibraryManagementSystem.Console.Database;

namespace LibraryManagementSystem.Tests;

/// <summary>
/// Тестирует подключение к БД.
/// </summary>
public class DatabaseTests
{
    [Fact]
    public void Database_Should_Connect_Successfully()
    {
        // Arrange
        DatabaseManager databaseManager = new();

        // Act
        using var connection =
            databaseManager.CreateConnection();

        // Assert
        Assert.NotNull(connection);

        Assert.Equal(
            System.Data.ConnectionState.Open,
            connection.State
        );
    }
}