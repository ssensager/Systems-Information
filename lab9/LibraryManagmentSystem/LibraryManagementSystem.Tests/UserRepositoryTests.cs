using LibraryManagementSystem.Console.Database;
using LibraryManagementSystem.Console.Models;
using LibraryManagementSystem.Console.Repositories;
using LibraryManagementSystem.Console.Utils;

namespace LibraryManagementSystem.Tests;

/// <summary>
/// Тестирует CRUD операции пользователей.
/// </summary>
public class UserRepositoryTests
{
    private readonly IUserRepository _repository;

    /// <summary>
    /// Инициализирует тестовый репозиторий.
    /// </summary>
    public UserRepositoryTests()
    {
        DatabaseManager databaseManager = new();

        _repository = new UserRepository(databaseManager);

        TestHelper.CleanupUsers();
    }

    /// <summary>
    /// Проверяет добавление пользователя.
    /// </summary>
    [Fact]
    public void Create_Should_Add_User()
    {
        // Arrange
        User user = new()
        {
            Username = "test_user",
            PasswordHash = PasswordHasher.HashPassword("12345"),
            Email = "test@test.com",
        };

        // Act
        int id = _repository.Create(user);

        User? createdUser = _repository.GetById(id);

        // Assert
        Assert.NotNull(createdUser);

        Assert.Equal("test_user", createdUser!.Username);
    }

    /// <summary>
    /// Проверяет получение всех пользователей.
    /// </summary>
    [Fact]
    public void GetAll_Should_Return_Users()
    {
        // Arrange
        User user = new()
        {
            Username = "reader",
            PasswordHash = PasswordHasher.HashPassword("12345"),
            Email = "reader@test.com",
        };

        _repository.Create(user);

        // Act
        var users = _repository.GetAll();

        // Assert
        Assert.NotEmpty(users);
    }

    /// <summary>
    /// Проверяет обновление пользователя.
    /// </summary>
    [Fact]
    public void Update_Should_Modify_User()
    {
        // Arrange
        User user = new()
        {
            Username = "before_update",
            PasswordHash = PasswordHasher.HashPassword("12345"),
            Email = "before@test.com",
        };

        int id = _repository.Create(user);

        User? existingUser = _repository.GetById(id);

        Assert.NotNull(existingUser);

        existingUser!.Username = "updated_user";

        existingUser.Email = "updated@test.com";

        // Act
        bool result = _repository.Update(existingUser);

        User? updatedUser = _repository.GetById(id);

        // Assert
        Assert.True(result);

        Assert.NotNull(updatedUser);

        Assert.Equal("updated_user", updatedUser!.Username);
    }

    /// <summary>
    /// Проверяет удаление пользователя.
    /// </summary>
    [Fact]
    public void Delete_Should_Remove_User()
    {
        // Arrange
        User user = new()
        {
            Username = "delete_me",
            PasswordHash = PasswordHasher.HashPassword("12345"),
            Email = "delete@test.com",
        };

        int id = _repository.Create(user);

        // Act
        bool deleted = _repository.Delete(id);

        User? deletedUser = _repository.GetById(id);

        // Assert
        Assert.True(deleted);

        Assert.Null(deletedUser);
    }
}
