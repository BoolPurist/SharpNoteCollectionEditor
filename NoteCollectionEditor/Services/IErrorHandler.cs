using System;

namespace NoteCollectionEditor.Services;

public interface IErrorHandler
{
  void HandleExceptionAsError(Exception exception, string? errorMessage);
}