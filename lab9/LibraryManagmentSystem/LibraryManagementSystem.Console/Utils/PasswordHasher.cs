using BCrypt.Net;

namespace LibraryManagementSystem.Console.Utils;

/// <summary>
/// Выполняет хэширование паролей.
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Создает хэш пароля.
    /// </summary>
    /// <param name="password">Исходный пароль.</param>
    /// <returns>Хэшированный пароль.</returns>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Проверяет пароль.
    /// </summary>
    /// <param name="password">Исходный пароль.</param>
    /// <param name="hash">Хэш пароля.</param>
    /// <returns>
    /// true - пароль корректный.
    /// false - пароль неверный.
    /// </returns>
    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}