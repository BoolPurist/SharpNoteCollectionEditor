using System;
using System.Threading.Tasks;

namespace NoteCollectionEditor.Services;

public static class TaskUtilities
{
  // Way of handle async failure unhandled exceptions with error handler in sync context.
  // Link: https://johnthiriet.com/removing-async-void/#references
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
  public static async void FireAndForgetSafeAsync(
    this Task task, 
    IErrorHandler? handler = null
    , string? errorMessage = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
  {
    try
    {
      await task;
    }
    catch (Exception ex)
    {
      handler?.HandleExceptionAsError(ex, errorMessage);
    }
  }

}