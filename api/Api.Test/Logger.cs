using Microsoft.Extensions.Logging;

public class Logger
{
   public static List<string> Logs = new();
   
   public static List<string> Messages = new();

   public static string Text => string.Join(", ", Messages);
}

public class LoggerInMemory<T> : ILogger<T>
{
   public IDisposable? BeginScope<TState>(TState state)
      where TState : notnull => null;

   public bool IsEnabled(LogLevel logLevel) => true;

   public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
   {
      if (formatter == null) throw new ArgumentNullException(nameof(formatter));

      var message = formatter(state, exception);

      if (exception != null) message += $" | Exception: {exception.Message}";

      Logger.Logs.Add($"{DateTime.Now:O} [{logLevel}] {typeof(T).Name}: {message}");
      Logger.Messages.Add(message);
   }
}