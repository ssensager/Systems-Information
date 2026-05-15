namespace LibraryManagementSystem.Console.Models;

/// <summary>
/// Представляет книгу.
/// </summary>
public class Book
{
    /// <summary>
    /// ID книги.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Автор книги.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Год публикации.
    /// </summary>
    public int PublicationYear { get; set; }

    /// <summary>
    /// ID владельца книги.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}