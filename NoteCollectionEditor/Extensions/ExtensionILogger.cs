using Splat;

namespace NoteCollectionEditor.Extensions;

public static class ExtensionILogger
{
  public static void LogError(this ILogger logger, string message)
  {
    logger.Write(message, LogLevel.Error);
  }
  
  public static void LogWarning(this ILogger logger, string message)
  {
    logger.Write(message, LogLevel.Warn);
  }
  
  public static void LogFatal(this ILogger logger, string message)
  {
    logger.Write(message, LogLevel.Fatal);
  }
  
  public static void LogInfo(this ILogger logger, string message)
  {
    logger.Write(message, LogLevel.Info);
  }
  
  public static void LogDebug(this ILogger logger, string message)
  {
    logger.Write(message, LogLevel.Debug);
  }
}