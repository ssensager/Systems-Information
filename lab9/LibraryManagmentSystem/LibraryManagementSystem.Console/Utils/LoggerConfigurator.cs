using Serilog;

namespace LibraryManagementSystem.Console.Utils;

/// <summary>
/// Настраивает систему логирования.
/// </summary>
public static class LoggerConfigurator
{
    /// <summary>
    /// Инициализирует логгер приложения.
    /// </summary>
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                "Logs/log.txt",
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();
    }
}