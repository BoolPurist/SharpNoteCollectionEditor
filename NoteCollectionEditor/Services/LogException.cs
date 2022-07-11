using System;
using NoteCollectionEditor.Extensions;
using Splat;

namespace NoteCollectionEditor.Services;

public class LogException : IErrorHandler
{
  private readonly ILogger _logger;
  
  public LogException(ILogger logger)
  {
    _logger = logger;
  }
  

  public void HandleExceptionAsError(Exception exception, string? errorMessage)
  {
    if (errorMessage != null)
    {
      _logger.LogFatal(errorMessage);
    }
    _logger.LogFatal(exception.ToString());
    
  }
}