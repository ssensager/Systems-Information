using LibraryManagementSystem.Console.Database;
using LibraryManagementSystem.Console.Models;
using LibraryManagementSystem.Console.Repositories;
using LibraryManagementSystem.Console.Utils;
using Npgsql;

LoggerConfigurator.Configure();

DatabaseManager databaseManager = new();

IUserRepository userRepository = new UserRepository(databaseManager);

IBookRepository bookRepository = new BookRepository(databaseManager);

while (true)
{
    System.Console.WriteLine();
    System.Console.WriteLine("===== LIBRARY MENU =====");

    System.Console.WriteLine("1 - Add user");
    System.Console.WriteLine("2 - Show users");
    System.Console.WriteLine("3 - Update user");
    System.Console.WriteLine("4 - Delete user");

    System.Console.WriteLine("5 - Add book");
    System.Console.WriteLine("6 - Show books");

    System.Console.WriteLine("0 - Exit");

    System.Console.Write("Select: ");

    string? choice = System.Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddUser(userRepository);
            break;

        case "2":
            ShowUsers(userRepository);
            break;

        case "3":
            UpdateUser(userRepository);
            break;

        case "4":
            DeleteUser(userRepository);
            break;

        case "5":
            AddBook(bookRepository);
            break;

        case "6":
            ShowBooks(bookRepository);
            break;

        case "0":
            return;

        default:
            System.Console.WriteLine("Invalid option.");
            break;
    }
}

/// <summary>
/// Добавляет нового пользователя.
/// </summary>
/// <param name="repository">
/// Репозиторий пользователей.
/// </param>
static void AddUser(IUserRepository repository)
{
    try
    {
        System.Console.Write("Username: ");
        string username = System.Console.ReadLine()!;

        System.Console.Write("Password: ");
        string password = System.Console.ReadLine()!;

        System.Console.Write("Email: ");
        string email = System.Console.ReadLine()!;

        User user = new()
        {
            Username = username,
            PasswordHash = PasswordHasher.HashPassword(password),
            Email = email,
        };

        int id = repository.Create(user);

        System.Console.WriteLine($"User created. ID: {id}");
    }
    catch (PostgresException ex)
    {
        if (ex.SqlState == "23505")
        {
            System.Console.WriteLine("User with this username or email already exists.");
        }
        else
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}

/// <summary>
/// Отображает список пользователей.
/// </summary>
/// <param name="repository">
/// Репозиторий пользователей.
/// </param>
static void ShowUsers(IUserRepository repository)
{
    try
    {
        var users = repository.GetAll();

        foreach (var user in users)
        {
            System.Console.WriteLine($"{user.Id} | " + $"{user.Username} | " + $"{user.Email}");
        }
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Error loading users: {ex.Message}");
    }
}

/// <summary>
/// Обновляет данные пользователя.
/// </summary>
/// <param name="repository">
/// Репозиторий пользователей.
/// </param>
static void UpdateUser(IUserRepository repository)
{
    try
    {
        System.Console.Write("User ID: ");

        if (!int.TryParse(System.Console.ReadLine(), out int id))
        {
            System.Console.WriteLine("Invalid ID format.");

            return;
        }

        var user = repository.GetById(id);

        if (user is null)
        {
            System.Console.WriteLine("User not found.");

            return;
        }

        System.Console.Write("New username: ");
        user.Username = System.Console.ReadLine()!;

        System.Console.Write("New email: ");
        user.Email = System.Console.ReadLine()!;

        bool success = repository.Update(user);

        System.Console.WriteLine(success ? "User updated." : "Update failed.");
    }
    catch (PostgresException ex)
    {
        if (ex.SqlState == "23505")
        {
            System.Console.WriteLine("Username or email already exists.");
        }
        else
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}

/// <summary>
/// Удаляет пользователя.
/// </summary>
/// <param name="repository">
/// Репозиторий пользователей.
/// </param>
static void DeleteUser(IUserRepository repository)
{
    try
    {
        System.Console.Write("User ID: ");

        if (!int.TryParse(System.Console.ReadLine(), out int id))
        {
            System.Console.WriteLine("Invalid ID format.");

            return;
        }

        bool success = repository.Delete(id);

        System.Console.WriteLine(success ? "User deleted." : "User not found.");
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Delete error: {ex.Message}");
    }
}

/// <summary>
/// Добавляет новую книгу.
/// </summary>
/// <param name="repository">
/// Репозиторий книг.
/// </param>
static void AddBook(IBookRepository repository)
{
    try
    {
        System.Console.Write("Title: ");
        string title = System.Console.ReadLine()!;

        System.Console.Write("Author: ");
        string author = System.Console.ReadLine()!;

        System.Console.Write("Year: ");

        if (!int.TryParse(System.Console.ReadLine(), out int year))
        {
            System.Console.WriteLine("Invalid year format.");

            return;
        }

        System.Console.Write("Owner ID: ");

        if (!int.TryParse(System.Console.ReadLine(), out int ownerId))
        {
            System.Console.WriteLine("Invalid owner ID.");

            return;
        }

        Book book = new()
        {
            Title = title,
            Author = author,
            PublicationYear = year,
            OwnerId = ownerId,
        };

        int id = repository.Create(book);

        System.Console.WriteLine($"Book created. ID: {id}");
    }
    catch (PostgresException ex)
    {
        if (ex.SqlState == "23503")
        {
            System.Console.WriteLine("Owner with this ID does not exist.");
        }
        else
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}

/// <summary>
/// Отображает список книг.
/// </summary>
/// <param name="repository">
/// Репозиторий книг.
/// </param>
static void ShowBooks(IBookRepository repository)
{
    try
    {
        var books = repository.GetAll();

        foreach (var book in books)
        {
            System.Console.WriteLine(
                $"{book.Id} | "
                    + $"{book.Title} | "
                    + $"{book.Author} | "
                    + $"{book.PublicationYear}"
            );
        }
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Error loading books: {ex.Message}");
    }
}
