namespace LibraryManagementSystem.Console.Database;

/// <summary>
/// Хранит настройки подключения к PostgreSQL.
/// </summary>
public static class DbConfig
{
    /// <summary>
    /// Строка подключения к PostgreSQL.
    /// </summary>
    public const string CONNECTION_STRING =
        "Host=localhost;Port=5435;Database=library_db;Username=postgres;Password=postgres";
}