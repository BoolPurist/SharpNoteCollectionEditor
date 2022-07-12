using System;
using System.Collections.Generic;
using Splat;

namespace NoteCollectionEditor.Services;

public class InMemoryLogger : ILogger
{

  public IEnumerable<string> Logs => _logs;
  public LogLevel Level { get; }
  
  private readonly List<string> _logs = new();


  private void LogMessageIf(string message, LogLevel level)
  {
    if (CanEmitWithLevel(level))
    {
      LogMessage(message);
    }
  }
  
  private void LogMessageAndExceptionIf(string message, Exception exception, LogLevel level)
  {
    if (CanEmitWithLevel(level))
    {
      LogMessage(message);
      LogException(exception);
    }
  }
  
  public InMemoryLogger(LogLevel level = LogLevel.Debug) => Level = level;

  public void Write(string message, LogLevel logLevel) => LogMessageIf(message, logLevel);

  public void Write(Exception exception, string message, LogLevel logLevel) 
    => LogMessageAndExceptionIf(message, exception, logLevel);

  public void Write(string message, Type type, LogLevel logLevel) => LogMessage(message);

  public void Write(Exception exception, string message, Type type, LogLevel logLevel)
    => LogMessageAndExceptionIf(message, exception, logLevel);

  private bool CanEmitWithLevel(LogLevel logLevel) => (int) logLevel < (int) Level;

  private void LogMessage(string message) => _logs.Add(message); 

  private void LogException(Exception exception) => _logs.Add(exception.ToString());

}