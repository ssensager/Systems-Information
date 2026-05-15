namespace LibraryManagementSystem.Console.Models;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}